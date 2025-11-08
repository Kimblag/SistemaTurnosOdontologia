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
    public class CoberturaService
    {
        private readonly CoberturaRepositorio _repositorioCobertura;
        private readonly PlanRepositorio _repositorioPlan;
        private readonly CoberturaPorcentajeHistorialRepositorio _repositorioHistorial;



        public CoberturaService()
        {
            _repositorioCobertura = new CoberturaRepositorio();
            _repositorioPlan = new PlanRepositorio();
            _repositorioHistorial = new CoberturaPorcentajeHistorialRepositorio();
        }

        public List<CoberturaDto> Listar(string estado = null)
        {
            try
            {
                return CoberturaMapper.MapearListaADto(_repositorioCobertura.Listar(estado));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
                throw;
            }
        }

        public CoberturaDto ObtenerPorId(int idCobertura)
        {
            try
            {
                return CoberturaMapper.MapearADto(_repositorioCobertura.ObtenerPorId(idCobertura));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Modificar(CoberturaDto coberturaDto, TurnoService servicioTurno)
        {
            Cobertura coberturaModificada = CoberturaMapper.MapearAEntidad(coberturaDto);

            // obtener cobertura actual antes de modificar
            Cobertura coberturaActual = _repositorioCobertura.ObtenerPorId(coberturaDto.IdCobertura);
            bool nombreCambiado = !string.Equals(coberturaActual.Nombre.Trim(), coberturaDto.Nombre.Trim(), StringComparison.OrdinalIgnoreCase);

            // validar nombre duplicado solo si cambio el nombre
            if (nombreCambiado && _repositorioCobertura.ExistePorNombre(coberturaDto.Nombre))
            {
                throw new ExcepcionReglaNegocio($"Ya existe una cobertura con el nombre indicado: {coberturaDto.Nombre}");
            }

            bool seIntentaDarDeBaja =
                coberturaActual.Estado == EstadoEntidad.Activo &&
                coberturaModificada.Estado == EstadoEntidad.Inactivo;

            // validar si se intenta dar de baja y tiene turnos activos.
            if (seIntentaDarDeBaja && servicioTurno.TieneTurnosActivosPorCobertura(coberturaDto.IdCobertura))
            {
                throw new ExcepcionReglaNegocio("No se puede dar de baja la cobertura porque tiene turnos activos.");
            }

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.IniciarTransaccion();

                    _repositorioCobertura.Modificar(coberturaModificada, datos);
                    _repositorioPlan.ActualizarEstadoPorCobertura(coberturaModificada.IdCobertura, coberturaModificada.Estado, datos);

                    //porcentaje histórico
                    if (coberturaDto.PorcentajeCobertura.HasValue)
                    {
                        decimal? vigente = _repositorioHistorial.ObtenerPorcentajeActivo(coberturaDto.IdCobertura, datos);
                        if (vigente != coberturaDto.PorcentajeCobertura.Value)
                        {
                            _repositorioHistorial.CerrarPorcentajeActivo(coberturaDto.IdCobertura, datos);
                            _repositorioHistorial.InsertarNuevoPorcentaje(
                                coberturaDto.IdCobertura,
                                coberturaDto.PorcentajeCobertura.Value,
                                "Actualización manual desde edición de cobertura",
                                datos
                            );
                        }
                    }
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
                    throw new Exception("Error al intentar dar de baja la cobertura.");
                }
            }
        }

        public bool DarDeBaja(int idCobertura, TurnoService servicioTurno)
        {
            if (servicioTurno.TieneTurnosActivosPorCobertura(idCobertura))
            {
                // si tiene turnos activos no podemos dar de baja la cobertura.
                throw new ExcepcionReglaNegocio("No se puede dar de baja la cobertura porque tiene turnos activos.");
            }

            if (_repositorioCobertura.EstaDadoDeBaja(idCobertura))
            {
                // si ya está dado de baja, no puede volver a darse de baja
                throw new ExcepcionReglaNegocio("La cobertura ya se encuentra dada de baja.");
            }

            // inicio de la conexion desde acá para hacer una unidad de trabajo con transacciones
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.IniciarTransaccion();

                    _repositorioCobertura.DarDeBaja(idCobertura, 'I', datos);
                    _repositorioPlan.ActualizarEstadoPorCobertura(idCobertura, EstadoEntidad.Inactivo, datos);

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
                    throw new Exception("Error al intentar dar de baja la cobertura.");
                }
            }
        }

        public bool Crear(CoberturaDto nuevaCoberturaDto, List<PlanDto> listaPlanesDto)
        {
            // validar que no exista la cobertura con el mismo nombre
            if (_repositorioCobertura.ExistePorNombre(nuevaCoberturaDto.Nombre))
            {
                throw new ExcepcionReglaNegocio($"Ya existe una cobertura con el nombre indicado: {nuevaCoberturaDto.Nombre}");
            }

            Cobertura nuevaCobertura = CoberturaMapper.MapearAEntidad(nuevaCoberturaDto);

            // iniciar transaccion
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.IniciarTransaccion();

                    int idNuevaCobertura = _repositorioCobertura.Crear(nuevaCobertura, datos);
                    if (idNuevaCobertura == 0)
                    {
                        throw new Exception("No se pudo insertar la cobertura.");
                    }

                    if (listaPlanesDto.Count > 0)
                    {
                        nuevaCobertura.IdCobertura = idNuevaCobertura;
                        List<Plan> planes = PlanMapper.MapearAEntidad(listaPlanesDto, nuevaCobertura);
                        _repositorioPlan.Crear(planes, datos);
                    }
                    else if (nuevaCobertura.PorcentajeCobertura.HasValue)
                    {
                        _repositorioHistorial.InsertarNuevoPorcentaje(
                            idNuevaCobertura,
                            nuevaCobertura.PorcentajeCobertura.Value,
                            "Registro inicial de cobertura sin planes",
                            datos
                        );
                    }

                    datos.ConfirmarTransaccion();
                    return true;
                }
                catch (Exception)
                {
                    datos.RollbackTransaccion();
                    throw;
                }
            }
        }

        public bool EstaInactiva(int idCobertura)
        {
            try
            {
                return _repositorioCobertura.EstaDadoDeBaja(idCobertura);
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
