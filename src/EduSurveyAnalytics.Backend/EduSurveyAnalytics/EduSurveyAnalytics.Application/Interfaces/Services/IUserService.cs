using EduSurveyAnalytics.Application.DTO;
using EduSurveyAnalytics.Domain.Entities;
using EduSurveyAnalytics.Domain.Enums;

namespace EduSurveyAnalytics.Application.Interfaces.Services;

public interface IUserService
{
    /// <summary>
    /// Create new user
    /// </summary>
    /// <param name="accessCode">Login</param>
    /// <param name="lastName">Person's last name</param>
    /// <param name="firstName">Person's first name</param>
    /// <param name="middleName">Person's middle name (if exists)</param>
    /// <param name="birthDate">Person's birthdate</param>
    /// <param name="post">Person's job/post</param>
    /// <returns>Result with created and initialized user, or error</returns>
    Task<Result<User>> CreateUserAsync(string accessCode, string lastName, string firstName, string? middleName,
        DateOnly? birthDate, string? post);

    /// <summary>
    /// Delete existing user by id
    /// </summary>
    /// <param name="userId">Result with user's id to delete, or error</param>
    Task<Result<None>> DeleteUserAsync(Guid userId);

    /// <summary>
    /// Update user's data
    /// </summary>
    /// <param name="userId">User's id to update</param>
    /// <param name="accessCode">Login</param>
    /// <param name="lastName">Person's last name</param>
    /// <param name="firstName">Person's first name</param>
    /// <param name="middleName">Person's middle name (if exists)</param>
    /// <param name="birthDate">Person's birthdate</param>
    /// <param name="post">Person's job/post</param>
    /// <param name="permissions">List of allowed permissions</param>
    /// <returns></returns>
    Task<Result<None>> UpdateUserAsync(Guid userId, string accessCode, string lastName, string firstName,
        string? middleName, DateOnly? birthDate, string? post, IEnumerable<UserPermission> permissions);

    /// <summary>
    /// Change user's password
    /// </summary>
    /// <param name="userId">User's id to change password</param>
    /// <param name="password">Non-hashed password</param>
    /// <returns>Empty result with potential errors</returns>
    Task<Result<None>> SetUserPasswordAsync(Guid userId, string password);

    /// <summary>
    /// Whether user has a permission
    /// </summary>
    /// <param name="userId">User id</param>
    /// <param name="permission">User permission</param>
    /// <returns>Whether user has a permission</returns>
    Task<Result<bool>> UserHasPermissionAsync(Guid userId, UserPermission permission);

    /// <summary>
    /// Get user by access code
    /// </summary>
    /// <param name="accessCode">Access code to find</param>
    /// <param name="password">User's password</param>
    /// <returns>User found by access code and password, or error</returns>
    Task<Result<User>> GetUserByCredentialsAsync(string accessCode, string? password);

    /// <summary>
    /// Get user presentation data
    /// </summary>
    /// <param name="userId">User id to get</param>
    /// <returns>User's presentation data, or null if user not found</returns>
    Task<Result<UserPresentationDTO?>> GetUserPresentationAsync(Guid userId);

    /// <summary>
    /// Get user full data
    /// </summary>
    /// <param name="userId">User id to get</param>
    /// <returns>User's full data, or null if user not found</returns>
    Task<Result<UserFullDataDTO?>> GetUserFullDataAsync(Guid userId);
    
}