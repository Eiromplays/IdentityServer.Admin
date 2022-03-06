using Eiromplays.IdentityServer.Infrastructure.Identity.Entities;

namespace Eiromplays.IdentityServer.Infrastructure.Common.Helpers;

public class ProfilePictureHelper
{
    public static string GetProfilePicture(ApplicationUser user)
    {
        if (!string.IsNullOrWhiteSpace(user.ProfilePicture)) return user.ProfilePicture;

        var email = user.GravatarEmail ?? user.Email;

        return GetGravatarUrl(email);
    }

    private static string GetGravatarUrl(string? email, int? size = 150)
    {
        var hash = Md5HashHelper.GetMd5Hash(email);
        
        var sizeArgument = size > 0 ? $"?s={size}" : "";
        
        return $"https://www.gravatar.com/avatar/{hash}{sizeArgument}";
    }
}