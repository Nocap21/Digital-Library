using DigitalLibrary.API.Data;
using DigitalLibrary.API.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("The DefaultConnection connection string is not configured.");

builder.Services.AddDbContext<DigitalLibraryDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
var jwtKey = builder.Configuration["Jwt:Key"] ?? string.Empty;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDevelopment", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
            .AllowAnyHeader();
    });
});

var app = builder.Build();

if (string.IsNullOrWhiteSpace(jwtIssuer) || string.IsNullOrWhiteSpace(jwtAudience))
{
    throw new InvalidOperationException("JWT issuer and audience must be configured in the Jwt section.");
}

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException("JWT signing key is missing. Set the Jwt__Key environment variable.");
}

if (Encoding.UTF8.GetByteCount(jwtKey) < 32)
{
    throw new InvalidOperationException("JWT signing key must contain at least 32 UTF-8 bytes.");
}

var jwtExpirationMinutes = app.Configuration.GetValue<int>("Jwt:ExpirationMinutes");
if (jwtExpirationMinutes <= 0)
{
    throw new InvalidOperationException("Jwt:ExpirationMinutes must be a positive number.");
}

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<DigitalLibraryDbContext>();
    if (!await dbContext.Users.AnyAsync())
    {
        var username = app.Configuration["DIGITAL_LIBRARY_ADMIN_USERNAME"]?.Trim();
        var password = app.Configuration["DIGITAL_LIBRARY_ADMIN_PASSWORD"];
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
        {
            throw new InvalidOperationException(
                "The development database has no users. Set DIGITAL_LIBRARY_ADMIN_USERNAME and DIGITAL_LIBRARY_ADMIN_PASSWORD to create the initial user.");
        }

        if (username.Length > 50)
        {
            throw new InvalidOperationException("DIGITAL_LIBRARY_ADMIN_USERNAME must be no longer than 50 characters.");
        }

        var user = new User { Username = username, CreatedAt = DateTime.UtcNow };
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();
        user.PasswordHash = passwordHasher.HashPassword(user, password);
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
    }
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler(exceptionHandler => exceptionHandler.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
    var logger = context.RequestServices.GetRequiredService<ILoggerFactory>()
        .CreateLogger("GlobalExceptionHandler");
    logger.LogError(exception, "An unhandled exception occurred while processing a request.");

    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    await context.Response.WriteAsJsonAsync(new { message = "An unexpected error occurred." });
}));

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAngularDevelopment");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
