using Eiromplays.IdentityServer.Application.Identity.Sessions;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.BffUserSessions.Search;

public class Models
{
    public class Request
    {
        public UserSessionListFilter Data { get; set; } = default!;
    }
}