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
        private readonly TurnoService _servicioTurno;


        public CoberturaService()
        {
            _repositorioCobertura = new CoberturaRepositorio();
            _repositorioPlan = new PlanRepositorio();
            _servicioTurno = new TurnoService();
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
                return CoberturaMapper.MapearADto(_repositorioCobertura.ObtenerCoberturaPorId(idCobertura));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ModificarCobertura(CoberturaDto coberturaDto)
        {
            Cobertura cobertura = CoberturaMapper.MapearAEntidad(coberturaDto);

            // validar qu eno tenga turnos activos
            if (_servicioTurno.TieneTurnosActivosPorCobertura(coberturaDto.IdCobertura))
            {
                throw new ExcepcionReglaNegocio("No se puede dar de baja la cobertura porque tiene turnos activos.");
            }

            char nuevoEstado = coberturaDto.Estado.ToUpper()[0];
            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.IniciarTransaccion();
                    _repositorioCobertura.Modificar(cobertura, datos);
                    _repositorioPlan.ActualizarEstadoPorCobertura(cobertura.IdCobertura, nuevoEstado, datos);

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

        public bool DarDeBajaCobertura(int idCobertura)
        {
            if (_servicioTurno.TieneTurnosActivosPorCobertura(idCobertura))
            {
                // si tiene turnos activos no podemos dar de baja la cobertura.
                throw new ExcepcionReglaNegocio("No se puede dar de baja la cobertura porque tiene turnos activos.");
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


    }
}
