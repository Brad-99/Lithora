using Lithora.Api.Models;

namespace Lithora.Api.Dtos;

public record InspectionCreateDto(
    string MachineId,
    string? PhotoLot,
    DateTime? InspectedAt,
    InspectionResult Result
);

public record InspectionDto(
    Guid Id,
    string MachineId,
    string? PhotoLot,
    DateTime InspectedAt,
    InspectionResult Result
);
