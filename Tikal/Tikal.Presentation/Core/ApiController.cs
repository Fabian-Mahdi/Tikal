using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Tikal.Presentation.Core;

[ApiController]
public class ApiController : ControllerBase
{
    protected readonly ISender sender;

    public ApiController(ISender sender)
    {
        this.sender = sender;
    }
}