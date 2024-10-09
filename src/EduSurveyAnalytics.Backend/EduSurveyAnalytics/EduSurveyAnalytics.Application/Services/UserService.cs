using EduSurveyAnalytics.Application.Interfaces;
using EduSurveyAnalytics.Application.Interfaces.Services;
using EduSurveyAnalytics.Domain.Entities;
using EduSurveyAnalytics.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace EduSurveyAnalytics.Application.Services;

public class UserService(IApplicationDbContext context, IDateTimeProvider dateTimeProvider, IHashingProvider hashingProvider) : IUserService
{
    public async Task<Result<User>> CreateUserAsync(string accessCode, string lastName, string firstName, string? middleName, DateOnly? birthDate,
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
        var user = await context.Users.FindAsync(userId);

        if (user is null)
            return Result<None>.Error(ErrorCode.UserNotFound);

        context.Users.Remove(user);
        await context.SaveChangesAsync();

        return Result<None>.Success();
    }

    public async Task<Result<None>> UpdateUserAsync(Guid userId, string accessCode, string lastName, string firstName, string? middleName,
        DateOnly? birthDate, string? post, IEnumerable<UserPermission> permissions)
    {
        // If user with given access code already exists - error
        if (await context.Users.AnyAsync(u => u.AccessCode.Equals(accessCode)))
            return Result<None>.Error(ErrorCode.AccessCodeAlreadyExists);
        
        var user = await context.Users.AsTracking().FirstOrDefaultAsync(u => u.Id.Equals(userId));
        
        if (user is null)
            return Result<None>.Error(ErrorCode.UserNotFound);

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
        var user = await context.Users.AsTracking().FirstOrDefaultAsync(u => u.Id.Equals(userId));
        
        if (user is null)
            return Result<None>.Error(ErrorCode.UserNotFound);

        user.PasswordHash = hashingProvider.Hash(password);
        
        context.Users.Update(user);
        await context.SaveChangesAsync();

        return Result<None>.Success();
    }
}