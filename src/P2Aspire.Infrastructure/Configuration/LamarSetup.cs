using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using Hyperbee.Pipeline.Context;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using P2Aspire.Core.Identity;
using P2Aspire.Core.Validators;
using P2Aspire.Infrastructure.Data;
using P2Aspire.Infrastructure.IoC;



namespace P2Aspire.Infrastructure.Configuration;

public static class LamarSetup
{
    public static void ConfigureLamar( IHostBuilder builder )
    {
        builder.UseLamar( registry =>
        {
            registry.Scan( scan =>
            {
                scan.AssemblyContainingType<RegisterServiceAttribute>();

                scan.WithDefaultConventions();
                scan.ConnectImplementationsToTypesClosing( typeof( IValidator<> ) );
                scan.Convention<RegisterServiceConvention>();
            } );

            // Manual helper registrations
            registry.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            registry.AddSingleton<IPrincipalProvider, PrincipalProvider>();
            registry.AddSingleton<IPipelineContextFactory, PipelineContextFactory>();
            registry.AddSingleton<IValidatorProvider, ValidatorProvider>();


            // MVC + JSON settings
            registry.AddControllers()
                    .AddJsonOptions( opts =>
                    {
                        opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                        opts.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                        opts.JsonSerializerOptions.Converters.Add( new JsonStringEnumConverter() );
                        opts.JsonSerializerOptions.Converters.Add( new JsonBoolConverter() );
                        opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    } );
        } );
    }
}
