using Microsoft.AspNetCore.Mvc;

namespace JayyTee.MinimialApi;

[ApiController]
[Route("{controller}")]
public class DummyController : ControllerBase
{
    [HttpGet]
    public IActionResult GetDummy()
    {
        return Ok("Hello world");
    }
}