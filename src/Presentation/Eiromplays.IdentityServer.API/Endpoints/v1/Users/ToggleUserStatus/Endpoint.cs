using Eiromplays.IdentityServer.Application.Identity.Users;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.ToggleUserStatus;

public class Endpoint : Endpoint<Models.Request>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Post("/users/{Id}/toggle-status");
        Summary(s =>
        {
            s.Summary = "Toggle a user's active status.";
        });
        Version(1);
        Policies(EiaPermission.NameFor(EiaAction.Update, EiaResource.Users));
    }

    public override async Task HandleAsync(Models.Request req, CancellationToken ct)
    {
        if (req.Id != req.Data.UserId)
        {
            AddError("UserId and Id must match.");
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        await _userService.ToggleStatusAsync(req.Data, ct);

        await SendOkAsync(cancellation: ct);
    }
}