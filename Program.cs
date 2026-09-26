using System.Text;
using EmprestimoLibrary.Data;
using EmprestimoLibrary.Exceptions;
using EmprestimoLibrary.Repositories;
using EmprestimoLibrary.Repositories.interfaces;
using EmprestimoLibrary.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ControleEmprestimoLivroContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")));

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ILivroRepository, LivroRepository>();
builder.Services.AddScoped<IEmprestimoRepository, EmprestimoRepository>();
builder.Services.AddScoped<ICarteiraRepository, CarteiraRepository>();
builder.Services.AddScoped<IMovimentacaoCarteiraRepository, MovimentacaoCarteiraRepository>();
builder.Services.AddScoped<ICompraRepository, CompraRepository>();
builder.Services.AddScoped<IEditoraRepository, EditoraRepository>();

builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<ILivroService, LivroService>();
builder.Services.AddScoped<IEmprestimoService, EmprestimoService>();
builder.Services.AddScoped<ICarteiraService, CarteiraService>();
builder.Services.AddScoped<ICompraService, CompraService>();
builder.Services.AddScoped<IEditoraService, EditoraService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMetricasService, MetricasService>();

var jwtKey = builder.Configuration["Jwt:Key"]!;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("login", config =>
    {
        config.PermitLimit = 100;
        config.Window = TimeSpan.FromMinutes(15);
        config.QueueLimit = 0;
    });

    options.RejectionStatusCode = 429;
});

// CORS: permite requisicoes do frontend React em desenvolvimento
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000", "https://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Apenas redireciona para HTTPS fora do ambiente de desenvolvimento
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// CORS deve vir antes de Authentication/Authorization
app.UseCors("FrontendPolicy");

app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();