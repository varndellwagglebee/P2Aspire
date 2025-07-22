using Microsoft.Extensions.DependencyInjection;

namespace P2Aspire.Infrastructure.IoC;

[AttributeUsage( AttributeTargets.Class, Inherited = false )]
public sealed class RegisterServiceAttribute : Attribute
{
    public ServiceLifetime Lifetime { get; }

    public RegisterServiceAttribute( ServiceLifetime lifetime = ServiceLifetime.Transient )
        => Lifetime = lifetime;
}
