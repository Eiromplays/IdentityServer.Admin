using Eiromplays.IdentityServer.Application.Identity.PersistedGrants;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.PersistedGrants.Search;

public class Models
{
    public class Request
    {
        public PersistedGrantListFilter Data { get; set; } = default!;
    }
}