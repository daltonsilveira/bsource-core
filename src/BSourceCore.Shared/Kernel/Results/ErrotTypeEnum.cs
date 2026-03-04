namespace BSourceCore.Shared.Kernel.Results;
public enum ErrorType
{
    Validation,
    NotFound,
    Conflict,
    Forbidden,
    Unauthorized,
    BusinessRule,
    BadRequest,
    Unexpected
}