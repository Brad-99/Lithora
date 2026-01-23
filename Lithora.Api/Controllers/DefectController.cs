using Microsoft.AspNetCore.Mvc;
using Lithora.Api.Dtos;
using Lithora.Api.Services;

namespace Lithora.Api.Controllers;

[ApiController]
[Route("api/inspections/{inspectionId:guid}/defects")]
public class DefectsController : ControllerBase
{
    private readonly DefectService _service;

    public DefectsController(DefectService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<DefectDto>>> List(Guid inspectionId)
    {
        return Ok(await _service.ListByInspectionAsync(inspectionId));
    }

    [HttpPost]
    public async Task<ActionResult<DefectDto>> Create(
        Guid inspectionId,
        [FromBody] DefectCreateDto dto)
    {
        var created = await _service.AddAsync(inspectionId, dto);
        return created is null ? NotFound() : Ok(created);
    }
}
