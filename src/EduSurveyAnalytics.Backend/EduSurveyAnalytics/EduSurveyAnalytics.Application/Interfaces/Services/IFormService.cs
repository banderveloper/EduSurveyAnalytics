﻿using EduSurveyAnalytics.Application.DTO;

namespace EduSurveyAnalytics.Application.Interfaces.Services;

public interface IFormService
{
    Task<Result<None>> CreateFormAsync(Guid ownerId, string title, IEnumerable<FormFieldCreationDataDTO> formFields);
    Task<Result<FormPresentationDTO?>> GetFormPresentationByIdAsync(Guid formId);
    
}