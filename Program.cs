using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.interfaces;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.Dominio.Servicos;
using MinimalApi.DTOs;
using MinimalApi.Infraestrutura.Db;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(
  options => options.UseMySql(
    builder.Configuration.GetConnectionString("mysql")?.ToString(),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql")?.ToString())
  )
);

var app = builder.Build();

app.MapGet("/", () => Results.Json(new Home()));

app.MapPost("/login",([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) => {
  if (administradorServico.Login(loginDTO) != null)
    return Results.Ok("Login efetuado com sucesso!");
  else
    return Results.Unauthorized();
});

app.UseSwagger();
app.UseSwaggerUI();
app.Run();