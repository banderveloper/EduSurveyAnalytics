using EduSurveyAnalytics.Application;
using EduSurveyAnalytics.Application.DTO;
using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.Domain.Enums;
using EduSurveyAnalytics.WebApi.Models.Requests;
using EduSurveyAnalytics.WebApi.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduSurveyAnalytics.WebApi.Controllers;

/// <summary>
/// Form controller
/// </summary>
[Route("form")]
[Produces("application/json")]
public class FormController(IUserService userService, IFormService formService) : BaseController
{
    /// <summary>
    /// Create form with formFields. Requires JWT authorization.
    /// </summary>
    /// <param name="request">Request with form and form fields data</param>
    /// <returns>Empty result</returns>
    /// <response code="200">Success / not_permitted</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="422">Request is not valid</response>
    [Authorize]
    [HttpPost("create")]
    [ProducesResponseType(typeof(Result<CreateFormResponseModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<None>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result<Dictionary<string, string[]>>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<Result<CreateFormResponseModel>> CreateForm([FromBody] CreateFormRequestModel request)
    {
        // Check for permission for creating forms
        var hasPermission = (await userService.UserHasPermissionAsync(UserId, UserPermission.EditForms)).Data;

        if (!hasPermission)
            return Result<CreateFormResponseModel>.Error(ErrorCode.NotPermitted);

        var formFieldsDto = request.FormFields.Select(ff => new FormFieldCreationDataDTO
        {
            Constraints = ff.Constraints,
            Order = ff.Order,
            Title = ff.Title
        });

        var result = await formService.CreateFormAsync(UserId, request.FormTitle, formFieldsDto);

        return result.Succeed 
            ? Result<CreateFormResponseModel>.Success(new CreateFormResponseModel { FormId = result.Data }) 
            : Result<CreateFormResponseModel>.Error(result.ErrorCode);
    }

    /// <summary>
    /// Get form presentation data by id
    /// </summary>
    /// <param name="formId">Form id</param>
    /// <returns>Result with form presentation, or null</returns>
    /// <response code="200">Success / not_permitted</response>
    [ProducesResponseType(typeof(Result<FormPresentationDTO?>), StatusCodes.Status200OK)]
    [AllowAnonymous]
    [HttpGet("get/{formId:guid}")]
    public async Task<Result<FormPresentationDTO?>> GetForm(Guid formId)
    {
        var result = await formService.GetFormPresentationByIdAsync(formId);
        return result;
    }

    /// <summary>
    /// Get all forms shorts
    /// </summary>
    /// <returns>Result with forms shorts, or null</returns>
    /// <response code="200">Success</response>
    [ProducesResponseType(typeof(Result<GetFormsShortsResponseModel>), StatusCodes.Status200OK)]
    [AllowAnonymous]
    [HttpGet("get/shorts")]
    public async Task<Result<GetFormsShortsResponseModel>> GetFormsShorts()
    {
        var getResult = await formService.GetAllFormsShortInfo();
        return Result<GetFormsShortsResponseModel>.Success(new GetFormsShortsResponseModel
        {
            Forms = getResult.Data
        });
    }
}