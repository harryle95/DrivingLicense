using Microsoft.AspNetCore.Mvc;
using lib.Models;
using lib.Services;

namespace lib.Controllers;

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

    [HttpPost("Review")]
    public IActionResult MarkForReview(int[] Ids)
    {
        QuestionService.MarkForReview(Ids);
        return Ok();
    }

    [HttpPost("Import")]
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
