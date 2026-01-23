using Microsoft.EntityFrameworkCore;
using Lithora.Api.Data;
using Lithora.Api.Dtos;
using Lithora.Api.Models;

namespace Lithora.Api.Services;

public class InspectionService
{
    private readonly AppDbContext _db;

    public InspectionService(AppDbContext db) => _db = db;

    public async Task<List<InspectionDto>> ListAsync(string? machineId, DateTime? from, DateTime? to)
    {
        var q = _db.Inspections.AsQueryable();

        if (!string.IsNullOrWhiteSpace(machineId))
            q = q.Where(x => x.MachineId == machineId);

        if (from is not null)
            q = q.Where(x => x.InspectedAt >= from.Value);

        if (to is not null)
            q = q.Where(x => x.InspectedAt <= to.Value);

        return await q
            .OrderByDescending(x => x.InspectedAt)
            .Select(x => new InspectionDto(x.Id, x.MachineId, x.PhotoLot, x.InspectedAt, x.Result))
            .ToListAsync();
    }

    public async Task<InspectionDto?> GetAsync(Guid id)
    {
        return await _db.Inspections
            .Where(x => x.Id == id)
            .Select(x => new InspectionDto(x.Id, x.MachineId, x.PhotoLot, x.InspectedAt, x.Result))
            .FirstOrDefaultAsync();
    }

    public async Task<InspectionDto> CreateAsync(InspectionCreateDto dto)
    {
        var entity = new Inspection
        {
            MachineId = dto.MachineId,
            PhotoLot = dto.PhotoLot,
            InspectedAt = dto.InspectedAt ?? DateTime.UtcNow,
            Result = dto.Result
        };

        _db.Inspections.Add(entity);
        await _db.SaveChangesAsync();

        return new InspectionDto(entity.Id, entity.MachineId, entity.PhotoLot, entity.InspectedAt, entity.Result);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _db.Inspections.FindAsync(id);
        if (entity is null) return false;

        _db.Inspections.Remove(entity);
        await _db.SaveChangesAsync();
        return true;
    }
}
