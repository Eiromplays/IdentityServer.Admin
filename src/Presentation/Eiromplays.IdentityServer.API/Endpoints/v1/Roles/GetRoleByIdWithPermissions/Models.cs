using Eiromplays.IdentityServer.Application.Identity.Roles;

namespace Eiromplays.IdentityServer.API.Endpoints.v1.Roles.GetRoleByIdWithPermissions;

public class Models
{
    public class Request
    {
        public string? Id { get; set; }
    }

    public class Response
    {
        public RoleDto? RoleDto { get; set; }
    }
}