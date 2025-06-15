namespace IdentityAPI.Extensions;

public static class ResponseCookieExtensions
{
    public static IResponseCookies AddToken(this IResponseCookies cookies, string name, string value)
    {
        cookies.Append(name, value, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
        });

        return cookies;
    }
}