namespace Eiromplays.IdentityServer.Application.Identity.Users;

public class ConfirmPhoneNumberResponse
{
    public string? Message { get; set; }

    public string ReturnUrl { get; set; } = default!;
}