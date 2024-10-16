﻿using EduSurveyAnalytics.Application;
using EduSurveyAnalytics.Application.DTO;
using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.Domain.Enums;
using EduSurveyAnalytics.WebApi.Models.Requests;
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
    [HttpPost("delete")]
    [ProducesResponseType(typeof(Result<None>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<None>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result<Dictionary<string, string[]>>), StatusCodes.Status422UnprocessableEntity)]
    [Authorize]
    [HttpPost("create")]
    public async Task<Result<None>> CreateForm([FromBody] CreateFormRequestModel request)
    {
        // Check for permission for creating forms
        var hasPermission = (await userService.UserHasPermissionAsync(UserId, UserPermission.EditForms)).Data;

        if (!hasPermission)
            return Result<None>.Error(ErrorCode.NotPermitted);

        var formFieldsDto = request.FormFields.Select(ff => new FormFieldCreationDataDTO
        {
            Constraints = ff.Constraints,
            Order = ff.Order,
            Title = ff.Title
        });

        var result = await formService.CreateForm(UserId, request.FormTitle, formFieldsDto);
        return result;
    }
}