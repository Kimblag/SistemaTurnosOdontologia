using SGTO.Comun.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGTO.Negocio.Servicios.Exportacion
{
    public static class GeneradorCsv
    {
        public static byte[] GenerarReportePacientesCsv(List<ReportePacientesDto> lista)
        {
            if (lista == null || lista.Count == 0)
                throw new ArgumentException("No hay datos para exportar al CSV.");

            StringBuilder sb = new StringBuilder();

            // fila de encabezados
            sb.AppendLine("Nombre Completo;DNI;Cobertura;Plan;Total Turnos;Última Atención;Médico Frecuente");

            foreach (ReportePacientesDto p in lista)
            {
                string linea = string.Join(";",
                    LimpiarCsv(p.NombreCompleto),
                    LimpiarCsv(p.NumeroDocumento),
                    LimpiarCsv(p.Cobertura),
                    LimpiarCsv(p.Plan),
                    p.TotalTurnos.ToString(),
                    p.UltimaAtencion?.ToString("dd/MM/yyyy") ?? "-",
                    LimpiarCsv(p.MedicoFrecuente)
                );

                sb.AppendLine(linea);
            }

            byte[] bytes = Encoding.UTF8.GetPreamble()
                .Concat(Encoding.UTF8.GetBytes(sb.ToString()))
                .ToArray();

            return bytes;
        }

        private static string LimpiarCsv(string valor)
        {
            if (string.IsNullOrEmpty(valor)) return "-";
            // limpiar el texto; da error si dejamos saltos de línea, comas, espacios.
            return valor.Replace(";", ",").Replace("\r", " ").Replace("\n", " ").Trim();
        }

    }
}
