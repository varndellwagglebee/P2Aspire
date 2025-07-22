using P2Aspire.AppHost;

var builder = DistributedApplication.CreateBuilder( args );


var dbPassword = builder.AddParameter( "DbPassword", "postgres", true );
var dbUser = builder.AddParameter( "DbUser", "postgres", true );

var dbServer = builder.AddPostgres( "postgres", userName: dbUser, password: dbPassword )
    .PublishAsConnectionString()
    .WithDataVolume()
    .WithPgAdmin( x => x.WithImageTag( "9.5" ) );



var projectdb = dbServer.AddDatabase( "medical" );

var apiService = builder.AddProject<Projects.P2Aspire_Api>( "p2aspire-api" )
    .WithReference( projectdb )
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck( "/health" )
    .WithSwaggerUI();

builder.AddProject<Projects.P2Aspire_Migrations>( "p2aspire-migrations" )
    .WaitFor( projectdb )
    .WithReference( projectdb );

builder.Build().Run();
