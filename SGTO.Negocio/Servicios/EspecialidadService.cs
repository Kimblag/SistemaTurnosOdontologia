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
using System.Linq.Expressions;

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
                Especialidad entidadModificada = EspecialidadMapper.MapearAEntidad(dtoModificado);

                _repositorio.Modificar(entidadModificada);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DarDeBaja(int idEspecialidad, TurnoService servicioTurno)
        {
            if (servicioTurno.TieneTurnosActivosPorEspecialidad(idEspecialidad))
            {
                throw new ExcepcionReglaNegocio("No se puede dar de baja la especialidad porque tiene turnos activos");
            }

            if (_repositorio.EstaDadoDeBaja(idEspecialidad))
            {
   
            throw new ExcepcionReglaNegocio("La especialidad ya se encuentra dada de baja.");
            }

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.IniciarTransaccion();
                    _repositorio.DarDeBaja(idEspecialidad, 'I', datos);
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
                    throw new Exception("Error al intentar dar de baja la especialidad.");
                }

            }
        }
    }

}