using FluentValidation.Results;
using Hyperbee.Pipeline.Context;
using P2Aspire.Core.Extensions;
using P2Aspire.Core.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace P2Aspire.Core.Commands.Middleware;

public static class PipelineContextUserExtensions
{
    public static string GetUserEmail( this IPipelineContext context )
    {
        var principalProvider = context.ServiceProvider.GetService<IPrincipalProvider>();

        var email = principalProvider?.GetEmail();
        
        if ( email != null )
            return email;

        context.AddValidationResult( new ValidationFailure( "User", "Invalid User" ) );
        context.CancelAfter();

        return null;
    }
}
