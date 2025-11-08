using SGTO.Datos.Infraestructura;
using SGTO.Datos.Repositorios;
using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //  Llama al repositorio (capa de Datos)
            List<Especialidad> especialidades = _repositorio.Listar(estado);

            //  Mapea la lista de Entidades a DTOs (capa de Negocio)
            List<EspecialidadDto> dtos = EspecialidadMapper.MapearListaADto(especialidades);

            return dtos;
        }

        public void GuardarNuevaEspecialidad(EspecialidadDto nuevoDto)
        {
            //  Mapea el DTO a Entidad
            Especialidad nuevaEspecialidad = EspecialidadMapper.MapearAEntidad(nuevoDto);
            //  Llama al repositorio para guardar la nueva especialidad
            _repositorio.Crear(nuevaEspecialidad);
        }

        public EspecialidadDto ObtenerEspecialidadPorId(int idEspecialidad)
        {
            try
            {
                return EspecialidadMapper.MapearADto(_repositorio.ObtenerPorId(idEspecialidad));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ModificarEspecialidad(EspecialidadDto dtoModificado)
        {
            try
            {
                // 1. (Opcional pero recomendado: validación de duplicados,
                //    como la que vimos para "Crear o Reactivar")
                //    Por ahora lo hacemos simple.

                // 2. Mapea el DTO (que tiene los datos nuevos) a una Entidad
                Especialidad entidadModificada = EspecialidadMapper.MapearAEntidad(dtoModificado);

                // 3. Llama al repositorio con el método que creamos en el Paso 2.1
                _repositorio.Modificar(entidadModificada);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}