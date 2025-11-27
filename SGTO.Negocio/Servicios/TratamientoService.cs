using SGTO.Datos.Infraestructura;
using SGTO.Datos.Repositorios;
using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Mappers;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SGTO.Negocio.Servicios
{
    public class TratamientoService
    {
        private readonly TratamientoRepositorio _repositorio;
        private readonly EspecialidadRepositorio _repositorioEspecialidad;

        public TratamientoService()
        {
            _repositorio = new TratamientoRepositorio();
            _repositorioEspecialidad = new EspecialidadRepositorio();
        }


        public List<TratamientoDto> ObtenerTodosDto(string estado = null)
        {
            List<Tratamiento> tratamientos = _repositorio.Listar(estado);
            List<TratamientoDto> dtos = TratamientoMapper.MapearListaADto(tratamientos);
            return dtos;
        }

        public List<TratamientoDto> Listar(string estado = null)
        {
            try
            {
                return TratamientoMapper.MapearListaADto(_repositorio.Listar(estado));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
                throw;
            }
        }

        public TratamientoDto ObtenerTratamientoPorId(int idTratamiento)
        {
            try
            {
                return TratamientoMapper.MapearADto(_repositorio.ObtenerPorId(idTratamiento));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ModificarTratamiento(TratamientoDto dto)
        {
            ValidarEspecialidadActiva(dto.IdEspecialidad);

            Tratamiento tratamientoActual = _repositorio.ObtenerPorId(dto.IdTratamiento);

            if (tratamientoActual == null)
            {
                throw new ExcepcionReglaNegocio("El tratamiento que intenta modificar no existe.");
            }

            bool tieneUso = _repositorio.TieneHistoriaClinicaAsociada(dto.IdTratamiento);

            if (tieneUso)
            {
                if (tratamientoActual.Especialidad.IdEspecialidad != dto.IdEspecialidad)
                {
                    throw new ExcepcionReglaNegocio("No se puede cambiar la Especialidad porque el tratamiento tiene historial.");
                }

                if (tratamientoActual.CostoBase != dto.CostoBase)
                {
                    throw new ExcepcionReglaNegocio("Por integridad histórica, no se puede modificar el precio de un tratamiento ya facturado. Por favor, dé de baja este tratamiento y cree uno nuevo con el precio actualizado.");
                }
            }
            Tratamiento tratamientoModificado = TratamientoMapper.MapearAEntidad(dto);
            _repositorio.Modificar(tratamientoModificado);
        }

        public void GuardarNuevoTratamiento(TratamientoDto nuevoDto)
        {
            ValidarEspecialidadActiva(nuevoDto.IdEspecialidad);
            Tratamiento nuevoTratamiento = TratamientoMapper.MapearAEntidad(nuevoDto);
            _repositorio.Crear(nuevoTratamiento);
        }

        public bool DarDeBaja(int idTratamiento, TurnoService servicioTurno)
        {
            if (servicioTurno.TieneTurnosActivosPorTratamiento(idTratamiento))
            {
                throw new ExcepcionReglaNegocio("No se puede dar de baja el tratamiento porque tiene turnos activos.");
            }

            if (_repositorio.EstaDadoDeBaja(idTratamiento))
            {
                throw new ExcepcionReglaNegocio("El tratamiento ya se encuentra dado de baja.");
            }


            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.IniciarTransaccion();
                    char estadoInactivo = (char)EstadoEntidad.Inactivo; //

                    _repositorio.DarDeBaja(idTratamiento, estadoInactivo, datos);

                    datos.ConfirmarTransaccion();
                    return true;
                }
                catch (ExcepcionReglaNegocio)
                {
                    datos.RollbackTransaccion();
                    throw;
                }
                catch (Exception)
                {
                    datos.RollbackTransaccion();
                    throw new Exception("Error al intentar dar de baja el tratamiento.");
                }
            }
        }


        public List<TratamientoDto> ListarPorEspecialidad(int idEspecialidad)
        {
            try
            {
                if (idEspecialidad <= 0)
                    return new List<TratamientoDto>();

                List<Tratamiento> entidades = _repositorio.ListarPorEspecialidad(idEspecialidad);

                return TratamientoMapper.MapearListaADto(entidades);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR en TratamientoService.ListarPorEspecialidad: " + ex.Message);
                throw;
            }
        }


        private void ValidarEspecialidadActiva(int idEspecialidad)
        {
            var especialidad = _repositorioEspecialidad.ObtenerPorId(idEspecialidad);

            if (especialidad == null)
            {
                throw new ExcepcionReglaNegocio("La especialidad seleccionada no existe.");
            }

            if (especialidad.Estado == EstadoEntidad.Inactivo)
            {
                throw new ExcepcionReglaNegocio("No se puede asociar un tratamiento a una especialidad que está Inactiva.");
            }
        }

    }
}