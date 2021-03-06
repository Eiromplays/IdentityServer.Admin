using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.UpdateUser;

public class Endpoint : Endpoint<Models.Request>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Put("/users/{Id}");
        Summary(s =>
        {
            s.Summary = "Update a user.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Update, EiaResource.Users));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        if (User.GetUserId() == req.Id) ThrowError("You cannot update your own user.");

        await _userService.UpdateAsync(req.UpdateUserRequest, req.Id, ct);

        await SendNoContentAsync(cancellation: ct);
    }
}