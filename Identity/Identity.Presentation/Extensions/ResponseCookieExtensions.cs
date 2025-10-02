using Microsoft.AspNetCore.Http;

namespace Identity.Presentation.Extensions;

public static class ResponseCookieExtensions
{
    public static void AddRefreshToken(this IResponseCookies cookies, string value)
    {
        cookies.Append(
            "refresh_token",
            value,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            }
        );
    }
}