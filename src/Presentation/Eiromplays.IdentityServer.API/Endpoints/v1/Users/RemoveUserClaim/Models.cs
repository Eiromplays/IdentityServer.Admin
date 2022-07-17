using Eiromplays.IdentityServer.Application.Identity.Users.Claims;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Users.RemoveUserClaim;

public class Models
{
    public class Request
    {
        public string Id { get; set; } = default!;

        public RemoveUserClaimRequest RemoveUserClaimRequest { get; set; } = default!;
    }

    public class Response
    {
        public string? Message { get; set; }
    }
}