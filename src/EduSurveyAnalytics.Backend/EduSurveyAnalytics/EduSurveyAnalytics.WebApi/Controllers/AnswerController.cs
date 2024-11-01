using EduSurveyAnalytics.Application;
using EduSurveyAnalytics.Application.DTO;
using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.WebApi.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduSurveyAnalytics.WebApi.Controllers;

/// <summary>
/// Answer controller
/// </summary>
[Route("answer")]
[Produces("application/json")]
public class AnswerController(IAnswerService answerService) : BaseController
{
    /// <summary>
    /// Create answer for form
    /// </summary>
    /// <param name="request">Request data with answer</param>
    /// <returns>Empty result</returns>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="422">Request is not valid</response>
    [Authorize]
    [HttpPost("create")]
    [ProducesResponseType(typeof(Result<None>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<None>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result<Dictionary<string, string[]>>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<Result<None>> CreateAnswer([FromBody] CreateAnswerRequestModel request)
    {
        var result = await answerService.CreateAnswerAsync(UserId, request.FormId,
            request.FieldAnswers.Select(fa => new FieldAnswerCreationDataDTO
            {
                Value = fa.Value,
                FormFieldId = fa.FormFieldId
            }));

        return result;
    }

    [HttpGet("get/{formId:guid}")]
    public async Task<Result<FormAnswersPresentationDTO?>> GetFormAnswers(Guid formId)
    {
        var result = await answerService.GetFormAnswers(formId);
        return result;
    }
}