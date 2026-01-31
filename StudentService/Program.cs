using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentService.Data;
using StudentService.Infrastructure;
using StudentService.Interfaces;
using StudentService.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// -------------------- SERVICES --------------------

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// Service
builder.Services.AddScoped<IStudentService, StudentServices>();

// JWT token generator
builder.Services.AddScoped<ITokenService, JwtTokenService>();

// Fake Kafka (dev)
builder.Services.AddSingleton<IEventPublisher, FakeEventPublisher>();

//  JWT AUTH CONFIG (THIS WAS MISSING)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
        ),
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// -------------------- PIPELINE --------------------

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

//  ORDER MATTERS
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();