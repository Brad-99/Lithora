namespace Lithora.Api.Dtos;

public record DefectCreateDto(
    string DefectType,
    int Severity,
    string? Description
);

public record DefectDto(
    Guid Id,
    string DefectType,
    int Severity,
    string? Description
);
