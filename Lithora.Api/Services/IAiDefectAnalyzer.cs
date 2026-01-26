using Lithora.Api.Models;

namespace Lithora.Api.Services;

public interface IAiDefectAnalyzer
{
    Task<string> ClassifyAsync(Defect defect);
}
