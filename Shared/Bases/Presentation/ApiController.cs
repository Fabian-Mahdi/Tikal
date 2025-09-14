using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Shared.Bases.Presentation;

public class ApiController : ControllerBase
{
    protected readonly ISender sender;

    public ApiController(ISender sender)
    {
        this.sender = sender;
    }
}