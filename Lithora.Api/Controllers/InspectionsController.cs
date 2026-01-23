using Microsoft.AspNetCore.Mvc;
using Lithora.Api.Dtos;
using Lithora.Api.Services;

namespace Lithora.Api.Controllers;

[ApiController]
[Route("api/inspections")]
public class InspectionsController : ControllerBase
{
    private readonly InspectionService _service;

    public InspectionsController(InspectionService service) => _service = service;

    // GET /api/inspections?machineId=M01&from=2026-01-01&to=2026-01-20
    [HttpGet]
    public async Task<ActionResult<List<InspectionDto>>> List(
        [FromQuery] string? machineId,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
    {
        var result = await _service.ListAsync(machineId, from, to);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<InspectionDto>> Get(Guid id)
    {
        var item = await _service.GetAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<InspectionDto>> Create([FromBody] InspectionCreateDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ok = await _service.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }
}
