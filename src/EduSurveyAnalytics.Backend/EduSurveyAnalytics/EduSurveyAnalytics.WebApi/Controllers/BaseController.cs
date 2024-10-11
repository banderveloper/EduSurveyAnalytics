using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace EduSurveyAnalytics.WebApi.Controllers;

// Parent class for all controllers
[ApiController]
[Route("[controller]/[action]")]
public class BaseController : ControllerBase
{
    // userId got from access token.
    internal Guid UserId => Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
};