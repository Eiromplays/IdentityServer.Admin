using Eiromplays.IdentityServer.Application.Identity.Users;
using Eiromplays.IdentityServer.Application.Identity.Users.Password;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Personal.SetPassword;

public class Endpoint : Endpoint<SetPasswordRequest, Models.Response>
{
    private readonly IUserService _userService;

    public Endpoint(IUserService userService)
    {
        _userService = userService;
    }

    public override void Configure()
    {
        Put("/personal/set-password");
        Summary(s =>
        {
            s.Summary = "Set password for currently logged in user.";
        });
        Version(1);
    }

    public override async Task HandleAsync(SetPasswordRequest req, CancellationToken ct)
    {
        if (User.GetUserId() is not { } userId || string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        Response.Message = await _userService.SetPasswordAsync(req, userId);

        await SendOkAsync(Response, cancellation: ct);
    }
}