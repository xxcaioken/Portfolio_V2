var builder = WebApplication.CreateBuilder(args);

// Authentication and Authorization
builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSection = builder.Configuration.GetSection("Authentication:Jwt");
        var issuer = jwtSection.GetValue<string>("Issuer");
        var audience = jwtSection.GetValue<string>("Audience");
        var secret = jwtSection.GetValue<string>("Secret");

        var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret ?? string.Empty));
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/auth/login", (LoginRequest request, IConfiguration config) =>
{
    var expectedUser = config.GetValue<string>("Authentication:DemoUser:Username");
    var expectedPass = config.GetValue<string>("Authentication:DemoUser:Password");

    if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
    {
        return Results.BadRequest(new { error = "Credenciais inválidas" });
    }

    if (!string.Equals(request.Username, expectedUser, StringComparison.Ordinal) ||
        !string.Equals(request.Password, expectedPass, StringComparison.Ordinal))
    {
        return Results.Unauthorized();
    }

    var jwtSection = config.GetSection("Authentication:Jwt");
    var issuer = jwtSection.GetValue<string>("Issuer");
    var audience = jwtSection.GetValue<string>("Audience");
    var secret = jwtSection.GetValue<string>("Secret");
    var lifetimeMinutes = jwtSection.GetValue<int>("TokenLifetimeMinutes");

    var signingKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret ?? string.Empty));
    var creds = new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, request.Username),
        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, request.Username)
    };

    var expiresAt = DateTime.UtcNow.AddMinutes(lifetimeMinutes > 0 ? lifetimeMinutes : 60);

    var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
        issuer: issuer,
        audience: audience,
        claims: claims,
        expires: expiresAt,
        signingCredentials: creds
    );

    var tokenString = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
    return Results.Ok(new { token = tokenString, expiresAt });
})
.WithName("Login");

app.MapGet("/auth/me", (System.Security.Claims.ClaimsPrincipal user) =>
{
    var name = user.Identity?.Name ?? "unknown";
    var claims = user.Claims.Select(c => new { c.Type, c.Value });
    return Results.Ok(new { name, claims });
})
.RequireAuthorization()
.WithName("Me");

app.Run();

internal record LoginRequest(string Username, string Password);
