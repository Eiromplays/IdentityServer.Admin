using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.GetUserRoles;

public class Endpoint : Endpoint<Models.Request, List<UserRoleDto>>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/users/{Id}/roles");
        Summary(s =>
        {
            s.Summary = "Get a user's roles.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.View, EiaResource.UserRoles));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response = await _userService.GetRolesAsync(req.Id, ct);

        await SendAsync(Response, cancellation: ct);
    }
}