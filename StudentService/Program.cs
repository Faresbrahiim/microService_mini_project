using Microsoft.EntityFrameworkCore;
using StudentService.Data;
using StudentService.Infrastructure;
using StudentService.Interfaces;
using StudentService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register repository
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// Register service
builder.Services.AddScoped<IStudentService, StudentServices>();
// DI for addDbContext ...
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IEventPublisher, KafkaEventPublisher>();

builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
