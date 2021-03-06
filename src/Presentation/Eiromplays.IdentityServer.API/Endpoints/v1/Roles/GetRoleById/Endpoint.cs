using Eiromplays.IdentityServer.Application.Identity.Roles;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.GetRoleById;

public class Endpoint : Endpoint<Models.Request, RoleDto>
{
    private readonly IRoleService _roleService;

    public Endpoint(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public override void Configure()
    {
        Get("/roles/{Id}");
        Summary(s =>
        {
            s.Summary = "Get role details.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.View, EiaResource.Roles));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response = await _roleService.GetByIdAsync(req.Id);

        await SendAsync(Response, cancellation: ct);
    }
}