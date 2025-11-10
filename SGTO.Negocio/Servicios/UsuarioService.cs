using System;
using SGTO.Datos.Repositorios;
using SGTO.Negocio.DTOs;
using System.Collections.Generic;
using SGTO.Negocio.Mappers;

namespace SGTO.Negocio.Servicios
{
    public class UsuarioService
    {
        private readonly UsuarioRepositorio _repositorioUsuario;
        private readonly RolRepositorio _repositorioRol;

        public UsuarioService()
        {
            _repositorioUsuario = new UsuarioRepositorio();
            _repositorioRol = new RolRepositorio();
        }

        public List<UsuarioListadoDto> Listar(string estado = null)
        {
            try
            {
                return UsuarioMapper.MapearListaAListadoDto(_repositorioUsuario.Listar(estado));
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
