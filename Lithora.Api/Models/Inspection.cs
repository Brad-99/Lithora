namespace PhotoQualityTracker.Api.Models;

public enum InspectionResult
{
    Pass = 0,
    Fail = 1
}

public class Inspection
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string MachineId { get; set; } = string.Empty;

    public string? PhotoLot { get; set; }

    public DateTime InspectedAt { get; set; } = DateTime.UtcNow;

    public InspectionResult Result { get; set; }

    public List<Defect> Defects { get; set; } = new();
}
