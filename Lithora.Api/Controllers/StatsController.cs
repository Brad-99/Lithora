using Microsoft.AspNetCore.Mvc;
using Lithora.Api.Data;
using Lithora.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Lithora.Api.Controllers;

[ApiController]
[Route("api/stats")]
public class StatsController : ControllerBase
{
    private readonly AppDbContext _db;

    public StatsController(AppDbContext db) => _db = db;

    [HttpGet("fail-rate")]
    public async Task<IActionResult> FailRate(
        [FromQuery] string machineId,
        [FromQuery] int days = 7)
    {
        var since = DateTime.UtcNow.AddDays(-days);

        var total = await _db.Inspections
            .CountAsync(i => i.MachineId == machineId && i.InspectedAt >= since);

        var fail = await _db.Inspections
            .CountAsync(i => i.MachineId == machineId &&
                             i.Result == InspectionResult.Fail &&
                             i.InspectedAt >= since);

        return Ok(new
        {
            machineId,
            total,
            fail,
            failRate = total == 0 ? 0 : (double)fail / total
        });
    }
}
