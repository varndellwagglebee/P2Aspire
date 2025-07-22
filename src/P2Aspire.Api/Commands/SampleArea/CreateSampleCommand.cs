
using Audit.Core;
using Hyperbee.Pipeline;
using Hyperbee.Pipeline.Commands;
using Hyperbee.Pipeline.Context;
using P2Aspire.Core.Commands;
using P2Aspire.Core.Commands.Middleware;
using P2Aspire.Core.Extensions;
using P2Aspire.Core.Identity;
using P2Aspire.Data.Abstractions.Entity;
using P2Aspire.Data.Abstractions.Services;
using P2Aspire.Data.Abstractions.Services.Models;


namespace P2Aspire.Api.Commands.SampleArea;

public record CreateSample( string Name, string Description );

public interface ICreateSampleCommand : ICommandFunction<CreateSample, SampleDefinition>;

public class CreateSampleCommand : ServiceCommandFunction<CreateSample, SampleDefinition>, ICreateSampleCommand
{
    private readonly ISampleService _sampleService;
    private readonly string _user;

    public CreateSampleCommand(
        ISampleService sampleService,
        IPrincipalProvider principalProvider,
        IPipelineContextFactory pipelineContextFactory,
        ILogger<CreateSampleCommand> logger ) :
        base( pipelineContextFactory, logger )
    {
        _sampleService = sampleService;
        _user = principalProvider.GetEmail();
    }

    protected override FunctionAsync<CreateSample, SampleDefinition> CreatePipeline()
    {
        return PipelineFactory
            .Start<CreateSample>()
            .WithLogging()
            .PipeAsync( CreateSampleAsync )
            .CancelOnFailure( Validate<Sample> )
            .PipeAsync( InsertSampleAsync )
            .Build();
    }

    private async Task<Sample> CreateSampleAsync( IPipelineContext context, CreateSample sample )
    {

        return await Task.FromResult( new Sample
        {
            Name = sample.Name,
            Description = sample.Description,
            CreatedBy = _user ?? string.Empty,
        } );
    }


    private async Task<SampleDefinition> InsertSampleAsync( IPipelineContext context, Sample sample )
    {
        using ( AuditScope.Create( "Sample:Create", () => sample ) )
        {
            sample.Id = await _sampleService.CreateSampleAsync( sample );

            var sampleDefinition = new SampleDefinition
            (

                sample.Id,

                sample.Name,
                sample.Description
            );
            return sampleDefinition;
        }
    }

}
