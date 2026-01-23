using Lithora.Api.Data;
using Lithora.Api.Models;

namespace Lithora.Api.Services;

public class MachineSimulatorService
{
    private static readonly string[] DefectTypes =
        { "Scratch", "Particle", "Blur", "OverlayError" };

    private readonly AppDbContext _db;
    private readonly Random _rand = new();

    public MachineSimulatorService(AppDbContext db) => _db = db;

    public async Task<Inspection> SimulateAsync(string machineId)
    {
        var isFail = _rand.NextDouble() < 0.25;

        var inspection = new Inspection
        {
            MachineId = machineId,
            Result = isFail ? InspectionResult.Fail : InspectionResult.Pass
        };

        if (isFail)
        {
            var defectCount = _rand.Next(1, 4);
            for (int i = 0; i < defectCount; i++)
            {
                inspection.Defects.Add(new Defect
                {
                    DefectType = DefectTypes[_rand.Next(DefectTypes.Length)],
                    Severity = _rand.Next(1, 6),
                    Description = "Simulated defect"
                });
            }
        }

        _db.Inspections.Add(inspection);
        await _db.SaveChangesAsync();

        return inspection;
    }
}
