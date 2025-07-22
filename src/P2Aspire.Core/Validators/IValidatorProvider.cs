using FluentValidation;

namespace P2Aspire.Core.Validators;

public interface IValidatorProvider
{
    IValidator<TPlugin> For<TPlugin>()
        where TPlugin : class;
}
