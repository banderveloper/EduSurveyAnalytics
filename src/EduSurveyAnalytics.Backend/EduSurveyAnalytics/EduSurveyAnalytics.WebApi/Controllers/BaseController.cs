using Microsoft.AspNetCore.Mvc;

namespace EduSurveyAnalytics.WebApi.Controllers;

// Parent class for all controllers
[ApiController]
[Route("[controller]/[action]")]
public class BaseController : ControllerBase;