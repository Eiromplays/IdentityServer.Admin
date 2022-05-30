﻿using Ardalis.Specification.EntityFrameworkCore;
using Duende.IdentityServer.EntityFramework.Entities;
using Eiromplays.IdentityServer.Application.Common.Events;
using Eiromplays.IdentityServer.Application.Common.Exceptions;
using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Common.Specification;
using Eiromplays.IdentityServer.Application.Identity.ApiResources;
using Eiromplays.IdentityServer.Domain.Identity;
using Eiromplays.IdentityServer.Infrastructure.Persistence.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Eiromplays.IdentityServer.Infrastructure.Identity.Services;

internal partial class ApiResourceService : IApiResourceService
{
    private readonly ApplicationDbContext _db;
    private readonly IStringLocalizer _t;
    private readonly ILogger _logger;
    private readonly IEventPublisher _events;
    
    public ApiResourceService(ApplicationDbContext db, IStringLocalizer<ApiResourceService> t,
        ILogger<ApiResourceService> logger,
        IEventPublisher events)
    {
        _db = db;
        _t = t;
        _logger = logger;
        _events = events;
    }
    
    public async Task<PaginationResponse<ApiResourceDto>> SearchAsync(ApiResourceListFilter filter, CancellationToken cancellationToken)
    {
        var spec = new EntitiesByPaginationFilterSpec<ApiResource>(filter);

        var apiResources = await _db.ApiResources
            .WithSpecification(spec)
            .ProjectToType<ApiResourceDto>()
            .ToListAsync(cancellationToken);

        var count = await _db.ApiResources
            .CountAsync(cancellationToken);

        return new PaginationResponse<ApiResourceDto>(apiResources, count, filter.PageNumber, filter.PageSize);
    }

    public async Task<ApiResourceDto> GetAsync(int apiResourceId, CancellationToken cancellationToken)
    {
        var apiScope = await FindApiResourceByIdAsync(apiResourceId, cancellationToken);
        
        return apiScope.Adapt<ApiResourceDto>();
    }
    
    public async Task UpdateAsync(UpdateApiResourceRequest request, int apiResourceId, CancellationToken cancellationToken)
    {
        var apiResource = await FindApiResourceByIdAsync(apiResourceId, cancellationToken);

        apiResource.Name = request.Name;
        apiResource.DisplayName = request.DisplayName;
        apiResource.Description = request.Description;
        apiResource.ShowInDiscoveryDocument = request.ShowInDiscoveryDocument;
        apiResource.AllowedAccessTokenSigningAlgorithms = request.AllowedAccessTokenSigningAlgorithms;
        apiResource.RequireResourceIndicator = request.RequireResourceIndicator;
        apiResource.Enabled = request.Enabled;
        apiResource.NonEditable = request.NonEditable;
        
        _db.ApiResources.Update(apiResource);

        var success = await _db.SaveChangesAsync(cancellationToken) > 0;

        await _events.PublishAsync(new ApiResourceUpdatedEvent(apiResource.Id));
        
        if (!success)
        {
            throw new InternalServerException(_t["Update ApiResource failed"],
                new List<string> { "Failed to update ApiResource" });
        }
    }
    
    public async Task DeleteAsync(int apiResourceId, CancellationToken cancellationToken)
    {
        var apiResource = await FindApiResourceByIdAsync(apiResourceId, cancellationToken);

        _db.ApiResources.Remove(apiResource);

        var success = await _db.SaveChangesAsync(cancellationToken) > 0;

        await _events.PublishAsync(new ApiResourceDeletedEvent(apiResource.Id));
        
        if (!success)
        {
            throw new InternalServerException(_t["Delete ApiResource failed"], new List<string>{ "Failed to delete ApiResource" });
        }
    }
    
    #region Entity Queries

    // TODO: Move to repository or something like that :)
    private async Task<ApiResource> FindApiResourceByIdAsync(int apiResourceId, CancellationToken cancellationToken)
    {
        var apiResource = await _db.ApiResources
            .Include(x => x.UserClaims)
            .Include(x => x.Scopes)
            .Where(x => x.Id == apiResourceId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        _ = apiResource ?? throw new NotFoundException(_t["ApiResource Not Found."]);

        return apiResource;
    }

    #endregion
}