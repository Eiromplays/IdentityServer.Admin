using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.GetUserById;

public class Endpoint : Endpoint<Models.Request, UserDetailsDto>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Get("/users/{Id}");
        Summary(s =>
        {
            s.Summary = "Get a user's details";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.View, EiaResource.Users));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        Response = await _userService.GetAsync(req.Id, ct);

        await SendAsync(Response, cancellation: ct);
    }
}