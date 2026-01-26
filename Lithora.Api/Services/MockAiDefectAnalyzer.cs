using Lithora.Api.Models;

namespace Lithora.Api.Services;

public class MockAiDefectAnalyzer : IAiDefectAnalyzer
{
    public Task<string> ClassifyAsync(Defect defect)
    {
        // Stub: future AI model integration point
        return Task.FromResult("Potential lithography alignment issue");
    }
}
