using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string GetConfig(string primaryKey, params string[] envFallbackKeys)
{
    string? v = builder.Configuration[primaryKey];
    if (!string.IsNullOrWhiteSpace(v))
        return v!;
    foreach (string key in envFallbackKeys)
    {
        string? ev = Environment.GetEnvironmentVariable(key);
        if (!string.IsNullOrWhiteSpace(ev))
            return ev!;
    }
    return string.Empty;
}

IConfigurationSection jwtSection = builder.Configuration.GetSection("Authentication:Jwt");
string issuer = GetConfig("Authentication:Jwt:Issuer", "Authentication__Jwt__Issuer", "AuthenticationJwt_Issuer");
string audience = GetConfig("Authentication:Jwt:Audience", "Authentication__Jwt__Audience", "AuthenticationJwt_Audience");
string secret = GetConfig("Authentication:Jwt:Secret", "Authentication__Jwt__Secret", "AuthenticationJwt_Secret");

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
// CORS (Dev + opcional Prod via env "ALLOWED_ORIGINS" ou config "Cors:AllowedOrigins")
string? allowedOriginsCsv = builder.Configuration["Cors:AllowedOrigins"] ?? Environment.GetEnvironmentVariable("ALLOWED_ORIGINS");
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("DevCors", p => p
        .WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
    if (!string.IsNullOrWhiteSpace(allowedOriginsCsv))
    {
        string[] origins = allowedOriginsCsv
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (origins.Length > 0)
        {
            opt.AddPolicy("ProdCors", p => p
                .WithOrigins(origins)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());
        }
    }
});

string? azureSql = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
if (string.IsNullOrWhiteSpace(azureSql))
{
    // Fallbacks: via config simples e env vars
    azureSql = builder.Configuration["ConnectionStrings:AZURE_SQL_CONNECTIONSTRING"]
        ?? Environment.GetEnvironmentVariable("ConnectionStrings__AZURE_SQL_CONNECTIONSTRING")
        ?? Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
}
if (string.IsNullOrWhiteSpace(azureSql))
{
    throw new InvalidOperationException("ConnectionStrings:AZURE_SQL_CONNECTIONSTRING não configurada. Defina a connection string do Azure SQL (App Service: Connection strings ou Application settings).");
}
builder.Services.AddDbContext<Portfolio_V2.Infrastructure.AppDbContext>(options =>
    options.UseSqlServer(
        azureSql,
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(60), errorNumbersToAdd: null);
        }
    )
);

builder.Services.AddScoped<Portfolio_V2.Infrastructure.Repositories.IUserRepository, Portfolio_V2.Infrastructure.Repositories.UserRepository>();
builder.Services.AddScoped<Portfolio_V2.Application.Services.IAuthService, Portfolio_V2.Application.Services.AuthService>();
builder.Services.AddScoped<Portfolio_V2.Application.Services.ITokenService, Portfolio_V2.Application.Services.TokenService>();
builder.Services.AddScoped<Portfolio_V2.Infrastructure.Repositories.IKeyTaskRepository, Portfolio_V2.Infrastructure.Repositories.KeyTaskRepository>();
builder.Services.AddScoped<Portfolio_V2.Infrastructure.Repositories.IAboutRepository, Portfolio_V2.Infrastructure.Repositories.AboutRepository>();
builder.Services.AddScoped<Portfolio_V2.Infrastructure.Repositories.IExperienceRepository, Portfolio_V2.Infrastructure.Repositories.ExperienceRepository>();
builder.Services.AddScoped<Portfolio_V2.Infrastructure.Repositories.IHabilityRepository, Portfolio_V2.Infrastructure.Repositories.HabilityRepository>();
builder.Services.AddScoped<Portfolio_V2.Infrastructure.Repositories.IAditionalInfoRepository, Portfolio_V2.Infrastructure.Repositories.AditionalInfoRepository>();
builder.Services.AddScoped<Portfolio_V2.Infrastructure.Repositories.IRecommendationLetterRepository, Portfolio_V2.Infrastructure.Repositories.RecommendationLetterRepository>();
builder.Services.AddScoped<Portfolio_V2.Infrastructure.Repositories.ITestimonialRepository, Portfolio_V2.Infrastructure.Repositories.TestimonialRepository>();
// Translation repositories
builder.Services.AddScoped<Portfolio_V2.Infrastructure.Repositories.IExperienceTranslationRepository, Portfolio_V2.Infrastructure.Repositories.ExperienceTranslationRepository>();
builder.Services.AddScoped<Portfolio_V2.Infrastructure.Repositories.IHabilityTranslationRepository, Portfolio_V2.Infrastructure.Repositories.HabilityTranslationRepository>();
builder.Services.AddScoped<Portfolio_V2.Infrastructure.Repositories.IAditionalInfoTranslationRepository, Portfolio_V2.Infrastructure.Repositories.AditionalInfoTranslationRepository>();
builder.Services.AddScoped<Portfolio_V2.Infrastructure.Repositories.IKeyTaskTranslationRepository, Portfolio_V2.Infrastructure.Repositories.KeyTaskTranslationRepository>();
builder.Services.AddScoped<Portfolio_V2.Infrastructure.Repositories.IAboutTranslationRepository, Portfolio_V2.Infrastructure.Repositories.AboutTranslationRepository>();
builder.Services.AddScoped<Portfolio_V2.Infrastructure.Repositories.ITestimonialTranslationRepository, Portfolio_V2.Infrastructure.Repositories.TestimonialTranslationRepository>();

// BLL services
builder.Services.AddScoped<Portfolio_V2.BLL.IAboutBll, Portfolio_V2.BLL.AboutBll>();
builder.Services.AddScoped<Portfolio_V2.BLL.IExperienceBll, Portfolio_V2.BLL.ExperienceBll>();
builder.Services.AddScoped<Portfolio_V2.BLL.IHabilityBll, Portfolio_V2.BLL.HabilityBll>();
builder.Services.AddScoped<Portfolio_V2.BLL.IAditionalInfoBll, Portfolio_V2.BLL.AditionalInfoBll>();
builder.Services.AddScoped<Portfolio_V2.BLL.IKeyTaskBll, Portfolio_V2.BLL.KeyTaskBll>();
builder.Services.AddScoped<Portfolio_V2.BLL.IRecommendationLetterBll, Portfolio_V2.BLL.RecommendationLetterBll>();
builder.Services.AddScoped<Portfolio_V2.BLL.ITestimonialBll, Portfolio_V2.BLL.TestimonialBll>();

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
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
if (!app.Environment.IsDevelopment() && !string.IsNullOrWhiteSpace(allowedOriginsCsv))
{
    app.UseCors("ProdCors");
}
app.MapControllers();
app.MapGet("/healthz", () => Results.Ok("OK"));

using (IServiceScope scope = app.Services.CreateScope())
{
    Portfolio_V2.Infrastructure.AppDbContext db = scope.ServiceProvider.GetRequiredService<Portfolio_V2.Infrastructure.AppDbContext>();

    var strategy = db.Database.CreateExecutionStrategy();
    await strategy.ExecuteAsync(async () =>
    {
        if (app.Environment.IsDevelopment())
        {
            db.Database.EnsureCreated();
        }
        else
        {
            db.Database.Migrate();
        }
        await Portfolio_V2.Infrastructure.Seed.DatabaseSeeder.SeedAsync(db);
    });

}

app.Run();
