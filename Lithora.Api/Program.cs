using Microsoft.EntityFrameworkCore;
using Lithora.Api.Data;
using Lithora.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<InspectionService>();
builder.Services.AddScoped<DefectService>();
builder.Services.AddScoped<MachineSimulatorService>();
builder.Services.AddScoped<IAiDefectAnalyzer, MockAiDefectAnalyzer>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});



builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Default"));
});

var app = builder.Build();

// Apply EF Core migrations at startup (creates SQLite tables if missing)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // disable for local http static frontend

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
