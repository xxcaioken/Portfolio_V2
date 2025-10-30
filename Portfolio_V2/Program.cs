using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IConfigurationSection jwtSection = builder.Configuration.GetSection("Authentication:Jwt");
string issuer = jwtSection.GetValue<string>("Issuer") ?? string.Empty;
string audience = jwtSection.GetValue<string>("Audience") ?? string.Empty;
string secret = jwtSection.GetValue<string>("Secret") ?? string.Empty;

if (secret.Length < 32)
{
    throw new InvalidOperationException("JWT Secret inválido. Defina um segredo com pelo menos 32 caracteres via variáveis de ambiente ou Secret Manager.");
}
if (string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
{
    throw new InvalidOperationException("JWT Issuer/Audience não configurados.");
}

SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

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

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Admin", policy => policy.RequireRole(Portfolio_V2.Domain.Models.Role.Admin.ToString()));

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("DevCors", p => p
        .WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

builder.Services.AddDbContext<Portfolio_V2.Infrastructure.AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<Portfolio_V2.Infrastructure.Repositories.IUserRepository, Portfolio_V2.Infrastructure.Repositories.UserRepository>();
builder.Services.AddScoped<Portfolio_V2.Application.Services.IAuthService, Portfolio_V2.Application.Services.AuthService>();
builder.Services.AddScoped<Portfolio_V2.Application.Services.ITokenService, Portfolio_V2.Application.Services.TokenService>();

builder.Services.AddOpenApi();

WebApplication app = builder.Build();

app.UseExceptionHandler();
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");
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

using (IServiceScope scope = app.Services.CreateScope())
{
    Portfolio_V2.Infrastructure.AppDbContext db = scope.ServiceProvider.GetRequiredService<Portfolio_V2.Infrastructure.AppDbContext>();
    if (app.Environment.IsDevelopment())
        db.Database.EnsureCreated();
    else
        db.Database.Migrate();
    await Portfolio_V2.Infrastructure.Seed.DatabaseSeeder.SeedAsync(db);

}

app.Run();
