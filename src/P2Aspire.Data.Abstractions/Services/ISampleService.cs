using P2Aspire.Data.Abstractions.Entity;
using P2Aspire.Data.Abstractions.Services.Models;


namespace P2Aspire.Data.Abstractions.Services;
public interface ISampleService
{
   
   Task<int> CreateSampleAsync( Sample sample );
   Task<SampleDefinition> GetSampleAsync(int sampleId );
   
   Task <SampleDefinition> UpdateSampleAsync(  Sample existing, int sampleId, string name, string description );
   
   
}
