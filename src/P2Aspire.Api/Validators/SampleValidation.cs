using FluentValidation;
using P2Aspire.Data.Abstractions.Entity;

namespace P2Aspire.Api.Validators;

public class SampleValidation : AbstractValidator<Sample>
{
    public SampleValidation()
    {
        RuleFor( x => x.Name )
            .NotEmpty()
            .NotNull()
            .WithMessage( $"{nameof( Sample.Name )} cannot be null or empty." );

    }
}
