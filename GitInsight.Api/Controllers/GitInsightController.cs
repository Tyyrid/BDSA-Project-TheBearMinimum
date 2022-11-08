using Microsoft.AspNetCore.Mvc;

namespace GitInsight.Api.Controllers;

[ApiController]
[Route("")]
public class GitInsightController : ControllerBase
{
    private readonly ILogger<GitInsightController> _logger;

    public GitInsightController(ILogger<GitInsightController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<string> Get()
    {
        return "test";
    }
}
