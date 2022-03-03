using Eiromplays.IdentityServer.Application.Common.Interfaces;
using FastEndpoints;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.UpdateUser;

public class Endpoint : Endpoint<Models.Request, Models.Response>
{
    private readonly IIdentityService _identityService;
    
    public Endpoint(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public override void Configure()
    {
        Verbs(Http.PUT);
        Routes("/users/{id}");
        Version(1);
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        var user = await _identityService.FindUserByIdAsync(req.Id);
        if (user is null)
            ThrowError("User not found");
        
        user.Email = req.Email;
        user.UserName = req.UserName;
        
        var (result, userId) = await _identityService.UpdateUserAsync(user);
        foreach (var error in result.Errors) AddError(error);
        
        ThrowIfAnyErrors();
        
        if (string.IsNullOrWhiteSpace(userId))
            ThrowError("User was not updated");
        
        await SendCreatedAtAsync("/users/{id}", userId, new Models.Response{UserDto = user}, ct);
    }
}