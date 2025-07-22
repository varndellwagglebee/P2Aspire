using FluentValidation.Results;

namespace P2Aspire.Core.Validators;

public class CustomValidationFailure : ValidationFailure
{
    public CustomValidationFailure( string propertyName, string errorMessage, string errorCode ) : base( propertyName, errorMessage, errorCode )
    {
        ErrorCode = errorCode;
    }
}
