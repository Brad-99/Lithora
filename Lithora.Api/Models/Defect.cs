namespace Lithora.Api.Models;

public class Defect
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid InspectionId { get; set; }

    public string DefectType { get; set; } = string.Empty;

    public int Severity { get; set; }  // 1-5

    public string? Description { get; set; }

    public Inspection? Inspection { get; set; }
}
