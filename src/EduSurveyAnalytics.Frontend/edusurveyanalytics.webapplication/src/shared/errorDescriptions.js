const ERROR_DESCRIPTIONS = {
    UNKNOWN: 'An unknown error occurred. Please try again later.',
    INVALID_MODEL: 'The provided model is invalid or not supported.',
    INVALID_CREDENTIALS: 'The credentials provided are incorrect. Please check and try again.',
    ACCESS_CODE_ALREADY_EXISTS: 'This access code is already in use. Please use a different code.',
    USER_NOT_FOUND: 'The specified user could not be found.',
    FORM_NOT_FOUND: 'The requested form does not exist.',
    INVALID_ACCESS_TOKEN: 'The access token provided is invalid or expired.',
    INVALID_REFRESH_TOKEN: 'The refresh token provided is invalid or expired.',
    INVALID_FINGERPRINT: 'The fingerprint authentication failed.',
    SESSION_NOT_FOUND: 'No active session was found for this request.',
    NOT_PERMITTED: 'You do not have permission to perform this action.',
};

export function getErrorMessage(errorCode) {
    return ERROR_DESCRIPTIONS[errorCode.toUpperCase()] || 'An unspecified error occurred.';
}