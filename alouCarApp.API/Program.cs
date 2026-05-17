using AlouCar.Aplicacao.Aplicacoes;
using AlouCar.Aplicacao.Interfaces;
using AlouCar.Repositorio.Contexto;
using AlouCar.Repositorio.Interfaces;
using AlouCar.Repositorio;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Aplicacao
builder.Services.AddScoped<IClienteAplicacao, ClienteAplicacao>();
builder.Services.AddScoped<IVeiculoAplicacao, VeiculoAplicacao>();
builder.Services.AddScoped<IServicoAplicacao, ServicoAplicacao>();

// Repositorio
builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
builder.Services.AddScoped<IVeiculoRepositorio, VeiculoRepositorio>();
builder.Services.AddScoped<IServicoRepositorio, ServicoRepositorio>();

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();