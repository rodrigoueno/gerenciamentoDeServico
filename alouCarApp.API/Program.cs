using AlouCar.Aplicacao.Aplicacoes;
using AlouCar.Aplicacao.Interfaces;
using AlouCar.Repositorio.Contexto;
using AlouCar.Repositorio.Interfaces;
using AlouCar.Repositorio;
using Microsoft.EntityFrameworkCore;
using alouCarApp.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Aplicacao
builder.Services.AddScoped<IClienteAplicacao, ClienteAplicacao>();
builder.Services.AddScoped<IVeiculoAplicacao, VeiculoAplicacao>();
builder.Services.AddScoped<IServicoAplicacao, ServicoAplicacao>();

// Repositorio
builder.Services.AddSingleton<DbConnectionFactory>();
builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
builder.Services.AddScoped<IVeiculoRepositorio, VeiculoRepositorio>();
builder.Services.AddScoped<IServicoRepositorio, ServicoRepositorio>();

// IA
builder.Services.AddHttpClient<ISugestaoIAService, SugestaoIAService>();
builder.Services.AddScoped<ISugestaoIAService, SugestaoIAService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5150")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Banco de dados
builder.Services.AddDbContext<AlouCarContexto>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();