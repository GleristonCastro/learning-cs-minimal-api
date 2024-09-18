using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.interfaces;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.Dominio.Servicos;
using MinimalApi.DTOs;
using MinimalApi.Infraestrutura.Db;

#region Builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();
builder.Services.AddScoped<IVeiculosServico, VeiculoServico>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(
  options => options.UseMySql(
    builder.Configuration.GetConnectionString("mysql")?.ToString(),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql")?.ToString())
  )
);

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home()));
#endregion

#region Administradores
app.MapPost("/administradores/login",([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) => {
  if (administradorServico.Login(loginDTO) != null)
    return Results.Ok("Login efetuado com sucesso!");
  else
    return Results.Unauthorized();
});
#endregion

#region Veiculos
app.MapPost("/veiculos",([FromBody] VeiculoDTO veiculoDTO, IVeiculosServico veiculoServico) => {
  var veiculo = new Veiculo
  {
    Nome = veiculoDTO.Nome,
    Marca = veiculoDTO.Marca,
    Ano = veiculoDTO.Ano
  };
  veiculoServico.Incluir(veiculo);

  return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
});

app.MapGet("/veiculos",([FromQuery] int? pagina, IVeiculosServico veiculoServico) => {
  var veiculos = veiculoServico.Todos(pagina);
  return Results.Ok(veiculos);
});
#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();
app.Run();
#endregion