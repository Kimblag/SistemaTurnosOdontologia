using SGTO.Datos.Infraestructura;
using SGTO.Datos.Repositorios;
using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs.ParametroSistema;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Mappers;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SGTO.Negocio.Servicios
{
    public class ParametroService
    {
        private readonly ParametroSistemaRepositorio _repositorioParametroSistema;

        public ParametroService()
        {
            _repositorioParametroSistema = new ParametroSistemaRepositorio();
        }

        public ParametroSistemaDto Obtener()
        {
            List<ParametroSistema> parametros = _repositorioParametroSistema.Listar();
            return ParametroSistemaMapper.MapearADto(parametros);
        }


        public void Guardar(ParametroSistemaDto dto)
        {
            if (dto.DuracionTurnoMinutos < 15)
                throw new ExcepcionReglaNegocio("La duración del turno no puede ser menor a 15 minutos.");

            if (dto.ReintentosEmail < 1 || dto.ReintentosEmail > 10)
                throw new ExcepcionReglaNegocio("Los reintentos de correo deben estar entre 1 y 10.");

            if (dto.PuertoCorreo <= 0)
                throw new ExcepcionReglaNegocio("El puerto SMTP no puede ser cero o negativo.");

            if (!TimeSpan.TryParse(dto.HoraInicio, out TimeSpan horaInicio))
                throw new ExcepcionReglaNegocio("La hora de inicio no tiene un formato válido (hh:mm).");

            if (!TimeSpan.TryParse(dto.HoraCierre, out TimeSpan horaFin))
                throw new ExcepcionReglaNegocio("La hora de cierre no tiene un formato válido (hh:mm).");

            if (horaFin <= horaInicio)
                throw new ExcepcionReglaNegocio("El horario de cierre debe ser posterior al horario de inicio.");


            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                try
                {
                    datos.IniciarTransaccion();

                    _repositorioParametroSistema.ActualizarValor("NombreClinica", dto.NombreClinica, datos);
                    _repositorioParametroSistema.ActualizarValor("DuracionTurnoMinutos", dto.DuracionTurnoMinutos.ToString(), datos);
                    _repositorioParametroSistema.ActualizarValor("HoraInicioJornada", dto.HoraInicio, datos);
                    _repositorioParametroSistema.ActualizarValor("HoraFinJornada", dto.HoraCierre, datos);
                    _repositorioParametroSistema.ActualizarValor("SMTP_Server", dto.ServidorCorreo, datos);
                    _repositorioParametroSistema.ActualizarValor("SMTP_Port", dto.PuertoCorreo.ToString(), datos);
                    _repositorioParametroSistema.ActualizarValor("Email_From", dto.EmailRemitente, datos);
                    _repositorioParametroSistema.ActualizarValor("ReintentosEmail", dto.ReintentosEmail.ToString(), datos);

                    datos.ConfirmarTransaccion();
                }
                catch (ExcepcionReglaNegocio)
                {
                    datos.RollbackTransaccion();
                    throw;
                }
                catch (Exception ex)
                {
                    datos.RollbackTransaccion();
                    Debug.WriteLine("Error al guardar parámetros: " + ex.Message);
                    throw new Exception("Error inesperado al guardar los parámetros del sistema.");
                }
            }
        }

    }
}
