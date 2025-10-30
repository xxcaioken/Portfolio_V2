using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtSection = builder.Configuration.GetSection("Authentication:Jwt");
var issuer = jwtSection.GetValue<string>("Issuer") ?? string.Empty;
var audience = jwtSection.GetValue<string>("Audience") ?? string.Empty;
var secret = jwtSection.GetValue<string>("Secret") ?? string.Empty;

if (secret.Length < 32)
{
    throw new InvalidOperationException("JWT Secret inválido. Defina um segredo com pelo menos 32 caracteres via variáveis de ambiente ou Secret Manager.");
}
if (string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
{
    throw new InvalidOperationException("JWT Issuer/Audience não configurados.");
}

var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

// Authentication and Authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        options.SaveToken = false;
        options.IncludeErrorDetails = builder.Environment.IsDevelopment();
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = signingKey,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddDbContext<Portfolio_V2.Infrastructure.AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<Portfolio_V2.Infrastructure.Repositories.IUserRepository, Portfolio_V2.Infrastructure.Repositories.UserRepository>();
builder.Services.AddScoped<Portfolio_V2.Application.Services.IAuthService, Portfolio_V2.Application.Services.AuthService>();
builder.Services.AddScoped<Portfolio_V2.Application.Services.ITokenService, Portfolio_V2.Application.Services.TokenService>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<Portfolio_V2.Infrastructure.AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run();
