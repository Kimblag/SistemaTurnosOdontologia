using SGTO.Datos.Repositorios;
using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.Mappers; 
using System.Collections.Generic;

namespace SGTO.Negocio.Servicios
{
    public class EspecialidadService
    {
        private readonly EspecialidadRepositorio _repositorio;

        public EspecialidadService()
        {
            _repositorio = new EspecialidadRepositorio();
        }

        
        public List<EspecialidadDto> ObtenerTodasDto(string estado = null)
        {
            // 1. Llama al repositorio (capa de Datos)
            List<Especialidad> especialidades = _repositorio.Listar(estado);

            // 2. Mapea la lista de Entidades a DTOs (capa de Negocio)
            List<EspecialidadDto> dtos = EspecialidadMapper.MapearListaADto(especialidades);

            return dtos;
        }

        
    }
}