using Audit.Core;
using Hyperbee.Pipeline;
using Hyperbee.Pipeline.Commands;
using Hyperbee.Pipeline.Context;
using P2Aspire.Core.Commands;
using P2Aspire.Core.Commands.Middleware;
using P2Aspire.Core.Extensions;
using P2Aspire.Data.Abstractions.Services;
using P2Aspire.Data.Abstractions.Services.Models;
using P2Aspire.Data.PostgreSql;

namespace P2Aspire.Api.Commands.SampleArea;

public record UpdateSample( int sampleId, string Name, string Description );

public interface IUpdateSampleCommand : ICommandFunction<UpdateSample, SampleDefinition>;

public class UpdateSampleCommand : ServiceCommandFunction<UpdateSample, SampleDefinition>, IUpdateSampleCommand
{
    private readonly ISampleService _sampleService;
    private readonly DatabaseContext _databaseContext;

    public UpdateSampleCommand(
        ISampleService sampleService,
        DatabaseContext databaseContext,
        IPipelineContextFactory pipelineContextFactory,
        ILogger<UpdateSampleCommand> logger )
        : base( pipelineContextFactory, logger )
    {
        _sampleService = sampleService;
        _databaseContext = databaseContext;
    }

    protected override FunctionAsync<UpdateSample, SampleDefinition> CreatePipeline()
    {
        return PipelineFactory
            .Start<UpdateSample>()
            .WithLogging()
            .CancelOnFailure( Validate<UpdateSample> )
            .PipeAsync( UpdateSampleAsync )
            .Build();
    }

    private async Task<SampleDefinition> UpdateSampleAsync( IPipelineContext context, UpdateSample update )
    {

        var original = await _databaseContext.Samples.FindAsync( update.sampleId ) ?? throw new Exception( "Sample not found" );

        //var original = await _sampleService.GetSampleAsync( update.Id );  // This is an issue with updating in the service as it is not being tracked by the context

        using ( AuditScope.Create( "Sample:Update", () => original ) )
        {
            var updatedSample = await _sampleService.UpdateSampleAsync( original, update.sampleId, update.Name, update.Description );

            return updatedSample;
        }
    }
}




