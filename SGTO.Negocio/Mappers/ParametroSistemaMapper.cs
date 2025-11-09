using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs.ParametroSistema;
using System.Collections.Generic;

namespace SGTO.Negocio.Mappers
{
    public static class ParametroSistemaMapper
    {
        public static ParametroSistemaDto MapearADto(List<ParametroSistema> parametros)
        {
            ParametroSistemaDto dto = new ParametroSistemaDto();

            foreach (ParametroSistema parametro in parametros)
            {
                if (parametro.Nombre == "NombreClinica")
                    dto.NombreClinica = parametro.Valor;
                else if (parametro.Nombre == "DuracionTurnoMinutos")
                    dto.DuracionTurnoMinutos = int.TryParse(parametro.Valor, out int dur) ? dur : 60;
                else if (parametro.Nombre == "HoraInicioJornada")
                    dto.HoraInicio = parametro.Valor;
                else if (parametro.Nombre == "HoraFinJornada")
                    dto.HoraCierre = parametro.Valor;
                else if (parametro.Nombre == "SMTP_Server")
                    dto.ServidorCorreo = parametro.Valor;
                else if (parametro.Nombre == "SMTP_Port")
                    dto.PuertoCorreo = int.TryParse(parametro.Valor, out int port) ? port : 587;
                else if (parametro.Nombre == "Email_From")
                    dto.EmailRemitente = parametro.Valor;
                else if (parametro.Nombre == "ReintentosEmail")
                    dto.ReintentosEmail = int.TryParse(parametro.Valor, out int retry) ? retry : 3;
                else if (parametro.Nombre == "Moneda")
                    dto.Moneda = parametro.Valor;
            }

            return dto;
        }
    }
}
