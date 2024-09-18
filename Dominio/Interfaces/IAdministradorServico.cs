using MinimalApi.Dominio.Entidades;
using MinimalApi.DTOs;

namespace MinimalApi.Dominio.interfaces;

public interface IAdministradorServico
{
  Administrador? Login(LoginDTO loginDTO);
};