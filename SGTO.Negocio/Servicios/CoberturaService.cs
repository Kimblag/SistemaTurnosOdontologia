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
    public class CoberturaService
    {
        private readonly CoberturaRepositorio _repositorioCobertura;
        private readonly PlanRepositorio _repositorioPlan;


        public CoberturaService()
        {
            _repositorioCobertura = new CoberturaRepositorio();
            _repositorioPlan = new PlanRepositorio();
        }

        public List<CoberturaDto> Listar(string estado = null)
        {
            try
            {
                return CoberturaMapper.MapearListaADto(_repositorioCobertura.Listar(estado));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CoberturaDto ObtenerCoberturaPorId(int idCobertura)
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

        public bool ModificarCobertura(CoberturaDto coberturaDto, TurnoService servicioTurno)
        {
            Cobertura cobertura = CoberturaMapper.MapearAEntidad(coberturaDto);

            // validar qu eno tenga turnos activos
            if (servicioTurno.TieneTurnosActivosPorCobertura(coberturaDto.IdCobertura))
            {
                throw new ExcepcionReglaNegocio("No se puede dar de baja la cobertura porque tiene turnos activos.");
            }
            Cobertura coberturaActual = _repositorioCobertura.ObtenerPorId(coberturaDto.IdCobertura);
            bool nombreCambiado = !string.Equals(coberturaActual.Nombre.Trim(), coberturaDto.Nombre.Trim(), StringComparison.OrdinalIgnoreCase);

            if (nombreCambiado && _repositorioCobertura.ExisteCobertura(coberturaDto.Nombre))
            {
                throw new ExcepcionReglaNegocio($"Ya existe una cobertura con el nombre indicado: {coberturaDto.Nombre}");
            }


            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.IniciarTransaccion();
                    _repositorioCobertura.Modificar(cobertura, datos);
                    _repositorioPlan.ActualizarEstadoPorCobertura(cobertura.IdCobertura, cobertura.Estado, datos);

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

        public bool DarDeBajaCobertura(int idCobertura, TurnoService servicioTurno)
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
                    _repositorioPlan.DarDeBaja(idCobertura, 'I', datos);

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

        public bool CrearCobertura(CoberturaDto coberturaDto, List<PlanDto> listaPlanesDto)
        {
            // validar que no exista la cobertura con el mismo nombre
            if (_repositorioCobertura.ExisteCobertura(coberturaDto.Nombre))
            {
                throw new ExcepcionReglaNegocio($"Ya existe una cobertura con el nombre indicado: {coberturaDto.Nombre}");
            }

            Cobertura nuevaCobertura = CoberturaMapper.MapearAEntidad(coberturaDto);

            // iniciar transaccion
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.IniciarTransaccion();
                    int idNuevaCobertura = _repositorioCobertura.Crear(nuevaCobertura, datos);
                    if (idNuevaCobertura == 0)
                    {
                        throw new Exception("Nos e pudo insertar la cobertura.");
                    }

                    if (listaPlanesDto.Count > 0)
                    {
                        nuevaCobertura.IdCobertura = idNuevaCobertura;
                        List<Plan> planes = PlanMapper.MapearAEntidad(listaPlanesDto, nuevaCobertura);
                        _repositorioPlan.Crear(planes, datos);
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


    }
}
