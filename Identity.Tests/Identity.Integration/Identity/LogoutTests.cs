using Identity.Integration.Core;

namespace Identity.Integration.Identity;

public class LogoutTests : IntegrationTestFixture
{
    // constants
    private const string logoutUri = "logout";

    [TestCase("test_cookie=test")]
    [TestCase("test_cookie_1=test", "test_cookie_2=test", "test_cookie_3=test")]
    [TestCase(
        "refresh_token=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJkZTMyYmVhMC1jOGIwLTRlMGQtYjkzYS1lZDkxZWIxMWJlMWMiLCJzdWIiOiIyIiwibmFtZSI6InRlc3Rpbmc4IiwibmJmIjoxNzYwMzc3ODI0LCJleHAiOjE3NjAzODE0MjQsImlhdCI6MTc2MDM3NzgyNCwiaXNzIjoiaHR0cHM6Ly9hdXRoLnRpa2Fsb25saW5lLmNvbSIsImF1ZCI6Imh0dHBzOi8vdGlrYWxvbmxpbmUuY29tIn0.ea-BbcKJ8OWj1ss6wqXtcQZWuKl85NQRFnpQzpbgUoc"
    )]
    public async Task Given_Cookies_When_Logout_Then_Expires_All_Cookies(params string[] cookies)
    {
        // given
        HttpRequestMessage request = new(HttpMethod.Post, logoutUri);

        request.Headers.Add("Cookie", string.Join(";", cookies));

        // when
        HttpResponseMessage response = await Client.SendAsync(request);

        IEnumerable<string> cookieHeaders = response.Headers.GetValues("Set-Cookie").ToList();

        // then
        foreach (string cookieHeader in cookieHeaders)
        {
            string[] parts = cookieHeader.Split(';');

            DateTime? expires = null;

            foreach (string part in parts)
            {
                if (part.StartsWith(" expires="))
                {
                    expires = DateTime.Parse(part[9..]);
                }
            }

            Assert.That(expires, Is.LessThan(DateTime.Now));
        }

        Assert.That(cookieHeaders.Count(), Is.EqualTo(cookies.Length));
    }
}