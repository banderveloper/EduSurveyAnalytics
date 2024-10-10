using EduSurveyAnalytics.Application.Interfaces;
using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.Domain.Entities;
using EduSurveyAnalytics.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EduSurveyAnalytics.Application.Services;

public class UserService(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IHashingProvider hashingProvider) : IUserService
{
    public async Task<Result<User>> CreateUserAsync(string accessCode, string lastName, string firstName,
        string? middleName, DateOnly? birthDate,
        string? post)
    {
        // If user with given access code already exists - error
        if (await context.Users.AnyAsync(u => u.AccessCode.Equals(accessCode)))
            return Result<User>.Error(ErrorCode.AccessCodeAlreadyExists);

        var newUser = new User
        {
            AccessCode = accessCode,
            LastName = lastName,
            FirstName = firstName,
            MiddleName = middleName,
            BirthDate = birthDate,
            Post = post,
            CreatedAt = dateTimeProvider.Now,
            UpdatedAt = dateTimeProvider.Now
        };

        await context.Users.AddAsync(newUser);
        await context.SaveChangesAsync();

        return Result<User>.Success(newUser);
    }

    public async Task<Result<None>> DeleteUserAsync(Guid userId)
    {
        // find user by id, error if not found
        var user = await context.Users.FindAsync(userId);

        if (user is null)
            return Result<None>.Error(ErrorCode.UserNotFound);

        // if user exists - delete it
        context.Users.Remove(user);
        await context.SaveChangesAsync();

        return Result<None>.Success();
    }

    public async Task<Result<None>> UpdateUserAsync(Guid userId, string accessCode, string lastName, string firstName,
        string? middleName, DateOnly? birthDate, string? post, IEnumerable<UserPermission> permissions)
    {
        // If user with given access code already exists - error
        if (await context.Users.AnyAsync(u => u.AccessCode.Equals(accessCode)))
            return Result<None>.Error(ErrorCode.AccessCodeAlreadyExists);

        // find user by userId with tracking for updating
        var user = await context.Users.AsTracking().FirstOrDefaultAsync(u => u.Id.Equals(userId));

        if (user is null)
            return Result<None>.Error(ErrorCode.UserNotFound);

        // update user's data
        user.AccessCode = accessCode;
        user.LastName = lastName;
        user.FirstName = firstName;
        user.MiddleName = middleName;
        user.BirthDate = birthDate;
        user.Post = post;
        user.Permissions = (ICollection<UserPermission>)permissions;

        context.Users.Update(user);
        await context.SaveChangesAsync();

        return Result<None>.Success();
    }

    public async Task<Result<None>> SetUserPasswordAsync(Guid userId, string password)
    {
        // find user by userId with tracking for updating
        var user = await context.Users.AsTracking().FirstOrDefaultAsync(u => u.Id.Equals(userId));

        if (user is null)
            return Result<None>.Error(ErrorCode.UserNotFound);

        // change user's password with hashing
        user.PasswordHash = hashingProvider.Hash(password);

        context.Users.Update(user);
        await context.SaveChangesAsync();

        return Result<None>.Success();
    }

    public async Task<Result<bool>> UserHasPermissionAsync(Guid userId, UserPermission permission)
    {
        // Find user with same id and needed permission (or all permissions)
        var hasPermission = await context.Users
            .AnyAsync(u => u.Id.Equals(userId) &&
                           (u.Permissions.Contains(UserPermission.All) ||
                            u.Permissions.Contains(permission)));

        return Result<bool>.Success(hasPermission);
    }

    public async Task<Result<User>> GetUserByCredentialsAsync(string accessCode, string? password)
    {
        // get user by access code
        var user = await context.Users.FirstOrDefaultAsync(u => u.AccessCode.Equals(accessCode));

        // if not found by access code - error
        if (user is null)
            return Result<User>.Error(ErrorCode.InvalidCredentials);

        // hash input password to find
        var passwordHash = password is null
            ? null
            : hashingProvider.Hash(password);

        // if password was set but not correct - error
        if (user.PasswordHash is not null && !user.PasswordHash.Equals(passwordHash))
            return Result<User>.Error(ErrorCode.InvalidCredentials);

        // If user was found and password or null or correct - success
        return Result<User>.Success(user);
    }
}