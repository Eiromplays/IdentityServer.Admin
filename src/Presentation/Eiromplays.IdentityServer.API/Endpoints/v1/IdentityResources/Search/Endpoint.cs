using Eiromplays.IdentityServer.Application.Common.Models;
using Eiromplays.IdentityServer.Application.Identity.IdentityResources;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.IdentityResources.Search;

public class Endpoint : Endpoint<IdentityResourceListFilter, PaginationResponse<IdentityResourceDto>>
{
    private readonly IIdentityResourceService _identityResourceService;

    public Endpoint(IIdentityResourceService identityResourceService)
    {
        _identityResourceService = identityResourceService;
    }

    public override void Configure()
    {
        Post("/identity-resources/search");
        Summary(s =>
        {
            s.Summary = "Search ApiScopes using available filters.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Search, EiaResource.ApiScopes));
    }

    public override async Task HandleAsync(IdentityResourceListFilter request, CancellationToken ct)
    {
        Response = await _identityResourceService.SearchAsync(request, ct);

        await SendAsync(Response, cancellation: ct);
    }
}