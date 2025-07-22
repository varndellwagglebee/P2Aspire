using Hyperbee.Pipeline.Commands;
using Hyperbee.Pipeline.Context;
using Microsoft.Extensions.Logging;

namespace P2Aspire.Core.Commands;

public abstract class ServiceCommandProcedure<TOutput>(
    IPipelineContextFactory pipelineContextFactory,
    ILogger logger )
    : CommandProcedure<TOutput>( pipelineContextFactory, logger );
