using Audit.Core;
using FluentValidation.Results;
using Hyperbee.Pipeline;
using Hyperbee.Pipeline.Commands;
using Hyperbee.Pipeline.Context;
using P2Aspire.Core.Commands;
using P2Aspire.Core.Commands.Middleware;
using P2Aspire.Core.Extensions;
using P2Aspire.Data.Abstractions.Services;
using P2Aspire.Data.Abstractions.Services.Models;


namespace P2Aspire.Api.Commands.SampleArea;


public interface IGetSampleCommand : ICommandFunction<int, SampleDefinition>;

public class GetSampleCommand : ServiceCommandFunction<int, SampleDefinition>, IGetSampleCommand
{
    private readonly ISampleService _sampleService;

    public GetSampleCommand(
        ISampleService sampleService,
        IPipelineContextFactory pipelineContextFactory,
        ILogger<GetSampleCommand> logger )
        : base( pipelineContextFactory, logger )
    {
        _sampleService = sampleService;
    }

    protected override FunctionAsync<int, SampleDefinition> CreatePipeline()
    {
        return PipelineFactory
            .Start<int>()
            .WithLogging()
            .PipeAsync( GetSampleAsync )
            .Build();
    }


    private async Task<SampleDefinition> GetSampleAsync( IPipelineContext context, int sampleId )
    {
        var sample = await _sampleService.GetSampleAsync( sampleId );

        if ( sample == null )
        {
            context.AddValidationResult( new ValidationFailure( nameof( sample ), "Sample does not exist" ) );
            context.CancelAfter();
            return null;
        }
        using var auditScope = AuditScope.Create( "Sample:Read", () => sample );

        return sample;
    }

}
