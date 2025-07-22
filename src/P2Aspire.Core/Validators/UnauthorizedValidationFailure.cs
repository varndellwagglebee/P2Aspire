using System.Net;
using FluentValidation.Results;

namespace P2Aspire.Core.Validators;

public class UnauthorizedValidationFailure : ValidationFailure
{
    public UnauthorizedValidationFailure( string propertyName, string errorMessage ) : base( propertyName, errorMessage )
    {
        ErrorCode = nameof(HttpStatusCode.Unauthorized);
    }
}
