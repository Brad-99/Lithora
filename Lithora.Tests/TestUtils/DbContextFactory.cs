using Microsoft.EntityFrameworkCore;
using Lithora.Api.Data;

namespace Lithora.Tests.TestUtils;

public static class DbContextFactory
{
    public static AppDbContext Create(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return new AppDbContext(options);
    }
}
