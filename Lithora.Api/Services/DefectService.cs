using Microsoft.EntityFrameworkCore;
using Lithora.Api.Data;
using Lithora.Api.Dtos;
using Lithora.Api.Models;

namespace Lithora.Api.Services;

public class DefectService
{
    private readonly AppDbContext _db;

    public DefectService(AppDbContext db) => _db = db;

    public async Task<List<DefectDto>> ListByInspectionAsync(Guid inspectionId)
    {
        return await _db.Defects
            .Where(d => d.InspectionId == inspectionId)
            .Select(d => new DefectDto(d.Id, d.DefectType, d.Severity, d.Description))
            .ToListAsync();
    }

    public async Task<DefectDto?> AddAsync(Guid inspectionId, DefectCreateDto dto)
    {
        var inspectionExists = await _db.Inspections.AnyAsync(i => i.Id == inspectionId);
        if (!inspectionExists) return null;

        var defect = new Defect
        {
            InspectionId = inspectionId,
            DefectType = dto.DefectType,
            Severity = dto.Severity,
            Description = dto.Description
        };

        _db.Defects.Add(defect);
        await _db.SaveChangesAsync();

        return new DefectDto(defect.Id, defect.DefectType, defect.Severity, defect.Description);
    }
}
