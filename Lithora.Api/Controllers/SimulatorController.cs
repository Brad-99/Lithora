using Microsoft.AspNetCore.Mvc;
using Lithora.Api.Services;

namespace Lithora.Api.Controllers;

[ApiController]
[Route("api/simulator")]
public class SimulatorController : ControllerBase
{
    private readonly MachineSimulatorService _service;

    public SimulatorController(MachineSimulatorService service) => _service = service;

    [HttpPost("machines/{machineId}")]
    public async Task<IActionResult> Simulate(string machineId)
    {
        var result = await _service.SimulateAsync(machineId);
        return Ok(result);
    }
}
