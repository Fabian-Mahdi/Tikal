using Identity.Presentation.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Presentation.Identity.Controllers.Logout;

[Route("logout")]
public class LogoutController : ApiController
{
    public LogoutController(ISender sender) : base(sender)
    {
    }

    [HttpPost]
    public IActionResult Logout()
    {
        foreach (KeyValuePair<string, string> cookie in Request.Cookies)
        {
            Response.Cookies.Delete(cookie.Key);
        }

        return Ok();
    }
}