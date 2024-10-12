namespace EduSurveyAnalytics.Domain.Enums;

/// <summary>
/// Allowed actions for user in our service
/// </summary>
public enum UserPermission
{
    // do everything (superadmin)
    All,
    // create, edit and delete users
    EditUsers,
    // edit user's permissions
    EditPermissions,
    // edit form's data (if user is not an owner)
    EditForms,
    // get responses from forms (if user is not an owner)
    GetFormsResponses,
    // get user's full data
    GetUsersFullData
}
