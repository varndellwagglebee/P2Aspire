#define CONTAINER_DIAGNOSTICS
using FluentValidation;
using Lamar;
using Microsoft.IdentityModel.Logging;
using P2Aspire.Api.Commands.SampleArea;
using P2Aspire.Api.Endpoints;
using P2Aspire.Api.Validators;
using P2Aspire.Core.Identity;
using P2Aspire.Core.Validators;
using P2Aspire.Data.Abstractions.Services;
using P2Aspire.Data.PostgreSql;
using P2Aspire.Data.PostgreSql.Services;
using P2Aspire.Infrastructure.Configuration;
using P2Aspire.Infrastructure.Extensions;
using P2Aspire.ServiceDefaults;

namespace P2Aspire.Api;

public class Startup : IStartupRegistry
{
    public void ConfigureServices( IHostApplicationBuilder builder, IServiceCollection services )
    {
        builder.UseStartup<P2Aspire.Infrastructure.Startup>();

        builder.AddBackgroundServices();
        builder.AddNpgsqlDbContext<DatabaseContext>( "medical" );
        //builder.AddOpenTelemetry();

        builder.Configuration
            .AddEnvironmentVariables()
            .AddUserSecrets<Program>( optional: true );
    }

    public void ConfigureApp( WebApplication app, IWebHostEnvironment env )
    {
        app.MapDefaultEndpoints();
        app.MapSampleEndpoints();

        ContainerDiagnostics( app, env );
    }

    public void ConfigureScanner( ServiceRegistry services )
    {
        IdentityModelEventSource.ShowPII = true; // show pii info in logs for debugging openid

        services.Scan( scanner =>
        {
            scanner.AssemblyContainingType<SampleValidation>(); // af
            scanner.ConnectImplementationsToTypesClosing( typeof( IValidator<> ) );
            scanner.TheCallingAssembly();
            scanner.WithDefaultConventions();
        } );

        // af
        services.For<ISampleService>().Use<SampleService>();
        services.For<IPrincipalProvider>().Use<PrincipalProvider>();
        services.For<ICreateSampleCommand>().Use<CreateSampleCommand>();
        services.For<IValidatorProvider>().Use<ValidatorProvider>();
    }

    private static void ContainerDiagnostics( IApplicationBuilder app, IHostEnvironment env )
    {
#if CONTAINER_DIAGNOSTICS
        if ( !env.IsDevelopment() )
            return;

        var container = (IContainer) app.ApplicationServices;
        Console.WriteLine( container.WhatDidIScan() );
        Console.WriteLine( container.WhatDoIHave() );
#endif
    }

}

internal static class StartupExtensions
{
    public static void AddBackgroundServices( this IHostApplicationBuilder builder )
    {
        /* example
        builder.Services.Configure<HeartbeatServiceOptions>( x =>
        {
            x.PeriodSeconds = 10;
            x.Text = "ka-thump";
        } );

        builder.Services.AddHostedService<HeartbeatService>();       
        */
    }


}


