using Microsoft.AspNetCore.Mvc;
using src.Models;
using src.Services;

namespace src.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionController : ControllerBase
{

    private readonly ILogger<QuestionController> _logger;

    public QuestionController(ILogger<QuestionController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetQuestions")]
    public IEnumerable<QuestionGetDTO> Get()
    {
        return QuestionService.GetQuestions();
    }

    [HttpPost("review")]
    public IActionResult MarkForReview(int[] Ids)
    {
        QuestionService.MarkForReview(Ids);
        return Ok();
    }

    [HttpPost("import")]
    public IActionResult ImportMarkdown(string filePath)
    {
        try
        {
            QuestionService.ImportMarkdown(filePath);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
