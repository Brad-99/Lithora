using Lithora.Api.Services;
using Lithora.Api.Dtos;
using Lithora.Api.Models;
using Lithora.Tests.TestUtils;
using Xunit;

namespace Lithora.Tests;

public class InspectionServiceTests
{
    [Fact]
    public async Task CreateInspection_ShouldPersistAndBeQueryable()
    {
        // Arrange
        var db = DbContextFactory.Create("inspection_db");
        var service = new InspectionService(db);

        var dto = new InspectionCreateDto(
            MachineId: "M01",
            PhotoLot: "LOT-001",
            InspectedAt: null,
            Result: InspectionResult.Pass
        );

        // Act
        var created = await service.CreateAsync(dto);
        var list = await service.ListAsync("M01", null, null);

        // Assert
        Assert.NotNull(created);
        Assert.Single(list);
        Assert.Equal("M01", list[0].MachineId);
        Assert.Equal(InspectionResult.Pass, list[0].Result);
    }
}