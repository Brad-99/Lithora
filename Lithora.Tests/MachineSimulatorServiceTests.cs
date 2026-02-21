using Lithora.Api.Services;
using Lithora.Api.Models;
using Lithora.Tests.TestUtils;
using Xunit;

namespace Lithora.Tests;

public class MachineSimulatorServiceTests
{
    [Fact]
    public async Task Simulator_ShouldCreateInspection()
    {
        // Arrange
        var db = DbContextFactory.Create("simulator_db");
        var service = new MachineSimulatorService(db);

        // Act
        var inspection = await service.SimulateAsync("SIM-01");

        // Assert
        Assert.NotNull(inspection);
        Assert.Equal("SIM-01", inspection.MachineId);

        if (inspection.Result == InspectionResult.Fail)
        {
            Assert.NotEmpty(inspection.Defects);
        }
    }
}