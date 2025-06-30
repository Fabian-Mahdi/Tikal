namespace IdentityAPI.Extensions;

public static class ResponseCookieExtensions
{
    public static IResponseCookies AddRefreshToken(this IResponseCookies cookies, string value)
    {
        cookies.Append("refreshToken", value, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
        });

        return cookies;
    }
}