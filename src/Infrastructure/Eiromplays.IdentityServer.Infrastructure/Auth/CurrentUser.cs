using System.Security.Claims;
using Eiromplays.IdentityServer.Application.Common.Interfaces;
using Shared.Authorization;

namespace Eiromplays.IdentityServer.Infrastructure.Auth;

public class CurrentUser : ICurrentUser, ICurrentUserInitializer
{
    private ClaimsPrincipal? _user;

    public string? Name => _user?.Identity?.Name;

    private string _userId = "";

    public string GetUserId() =>
        IsAuthenticated()
            ? _user?.GetUserId() ?? ""
            : _userId;

    public string? GetUserEmail() =>
        IsAuthenticated()
            ? ClaimsPrincipalExtensions.GetEmail(_user!)
            : string.Empty;

    public bool IsAuthenticated() =>
        _user?.Identity?.IsAuthenticated is true;

    public bool IsInRole(string role) =>
        _user?.IsInRole(role) is true;

    public IEnumerable<Claim>? GetUserClaims() =>
        _user?.Claims;

    public string? GetTenant() =>
        IsAuthenticated() ? _user?.GetTenant() : string.Empty;

    public void SetCurrentUser(ClaimsPrincipal user)
    {
        if (_user != null)
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        _user = user;
    }

    public void SetCurrentUserId(string userId)
    {
        if (!string.IsNullOrWhiteSpace(_userId))
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        if (!string.IsNullOrEmpty(userId))
        {
            _userId = userId;
        }
    }
}