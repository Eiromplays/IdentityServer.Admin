using System.Security.Claims;
using Eiromplays.IdentityServer.Application.Identity.Auth.Requests;
using Eiromplays.IdentityServer.Application.Identity.Auth.Responses.TwoFactorAuthentication;
using Eiromplays.IdentityServer.Application.Identity.Sessions;
using Eiromplays.IdentityServer.Application.Identity.Users.Logins;
using Eiromplays.IdentityServer.Application.Identity.Users.Password;

namespace Eiromplays.IdentityServer.Application.Identity.Users;

public interface IUserService : ITransientService
{
    Task<PaginationResponse<UserDetailsDto>> SearchAsync(UserListFilter filter, CancellationToken cancellationToken = default);

    Task<bool> ExistsWithNameAsync(string name);
    Task<bool> ExistsWithEmailAsync(string email, string? exceptId = null);
    Task<bool> ExistsWithPhoneNumberAsync(string phoneNumber, string? exceptId = null);

    Task<List<UserDetailsDto>> GetListAsync(CancellationToken cancellationToken);

    Task<int> GetCountAsync(CancellationToken cancellationToken);

    Task<UserDetailsDto> GetAsync(string userId, CancellationToken cancellationToken);

    Task<List<UserRoleDto>> GetRolesAsync(string userId, CancellationToken cancellationToken);
    Task<string> AssignRolesAsync(string userId, UserRolesRequest request, CancellationToken cancellationToken);

    Task<List<string>> GetPermissionsAsync(string userId, CancellationToken cancellationToken, bool includeUserClaims = true);
    Task<bool> HasPermissionAsync(string userId, string permission, CancellationToken cancellationToken = default);
    Task InvalidatePermissionCacheAsync(string userId, CancellationToken cancellationToken);

    Task ToggleStatusAsync(ToggleUserStatusRequest request, CancellationToken cancellationToken);

    Task<string> GetOrCreateFromPrincipalAsync(ClaimsPrincipal principal);
    Task<CreateUserResponse> CreateAsync(CreateUserRequest request, string origin);
    Task<CreateUserResponse> CreateExternalAsync(CreateExternalUserRequest request, string origin);
    Task UpdateAsync(UpdateUserRequest request, string userId, CancellationToken cancellationToken = default);

    Task<ConfirmEmailResponse> ConfirmEmailAsync(ConfirmEmailRequest request, string origin, CancellationToken cancellationToken = default);
    Task<ConfirmPhoneNumberResponse> ConfirmPhoneNumberAsync(string userId, string code);

    Task<string> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
    Task<string> ResetPasswordAsync(ResetPasswordRequest request);
    Task<string> ChangePasswordAsync(ChangePasswordRequest request, string userId);
    Task<string> SetPasswordAsync(SetPasswordRequest model, string userId);

    Task<bool> HasPasswordAsync(string userId);

    Task<ExternalLoginsResponse> GetExternalLoginsAsync(string userId);

    Task<List<UserLoginInfoDto>> GetLoginsAsync(string userId);

    Task<string> RemoveLoginAsync(RemoveLoginRequest model, string userId);

    Task<string> AddLoginAsync(string userId, UserLoginInfoDto login);

    Task<Stream> ExportPersonalDataAsync(string userId, bool includeLogins = true);

    Task<bool> RemoveBffSessionsAsync(string userId, CancellationToken cancellationToken = default);

    Task<List<UserSessionDto>> GetAllBffUserSessions(CancellationToken cancellationToken = default);
    Task<List<UserSessionDto>> GetBffUserSessionsAsync(string userId, CancellationToken cancellationToken = default);
    Task<UserSessionDto> GetBffUserSessionAsync(string key, string? userId = default, CancellationToken cancellationToken = default);
    Task<string> DeleteBffUserSessionAsync(string key, string? userId = default,  CancellationToken cancellationToken = default);

    Task<PaginationResponse<UserSessionDto>> SearchBffSessionsAsync(UserSessionListFilter filter, CancellationToken cancellationToken = default);

    Task DeleteAsync(string userId);

    Task<Result<GetEnableAuthenticatorResponse>> GetEnableTwoFactorAsync(string? userId);
    Task<Result<EnableAuthenticatorResponse>> EnableTwoFactorAsync(EnableAuthenticatorRequest req, ClaimsPrincipal claimsPrincipal);
    Task<string> DisableTwoFactorAsync(string? userId);
    Task<Result<TwoFactorAuthenticationResponse>> GetTwoFactorAuthenticationAsync(ClaimsPrincipal claimsPrincipal);
    Task<IList<string>> GetValidTwoFactorProvidersAsync(string? userId);
}