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

        public TratamientoService()
        {
            _repositorio = new TratamientoRepositorio();
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
            Tratamiento tratamiento = TratamientoMapper.MapearAEntidad(dto);
            _repositorio.Modificar(tratamiento);
        }
        public void GuardarNuevoTratamiento(TratamientoDto nuevoDto)
        {
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

    }
}