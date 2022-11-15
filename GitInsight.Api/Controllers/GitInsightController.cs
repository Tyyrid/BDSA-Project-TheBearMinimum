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

    [Route("{org}/{project}")]
    [HttpGet]
    public ActionResult<string> Get(string org, string project)
    {
        return org + "/" + project;
    }
}
