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

        public static byte[] GenerarReporteMedicosCsv(List<ReporteMedicosDto> lista)
        {
            if (lista == null || lista.Count == 0)
                throw new ArgumentException("No hay datos de médicos para exportar al CSV.");

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Matrícula;Nombre Completo;Especialidad;Estado;Total Turnos;Pacientes Atendidos;Último Turno");

            foreach (ReporteMedicosDto m in lista)
            {
                string linea = string.Join(";",
                    LimpiarCsv(m.Matricula),
                    LimpiarCsv(m.NombreCompleto),
                    LimpiarCsv(m.Especialidad),
                    LimpiarCsv(m.Estado),
                    m.TotalTurnos.ToString(),
                    m.PacientesAtendidos.ToString(),
                    m.UltimoTurno?.ToString("dd/MM/yyyy") ?? "-"
                );

                sb.AppendLine(linea);
            }

            return Encoding.UTF8.GetPreamble()
                .Concat(Encoding.UTF8.GetBytes(sb.ToString()))
                .ToArray();
        }


        public static byte[] GenerarReporteTurnosCsv(List<ReporteTurnosDto> lista)
        {
            if (lista == null || lista.Count == 0)
                throw new ArgumentException("No hay datos de turnos para exportar al CSV.");

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("IdTurno;Fecha;Hora;Paciente;DNI Paciente;Médico;Especialidad;Estado;Cobertura;Plan");

            foreach (ReporteTurnosDto t in lista)
            {
                string linea = string.Join(";",
                    t.IdTurno.ToString(),
                    t.Fecha.ToString("dd/MM/yyyy"),
                    t.Hora,
                    LimpiarCsv(t.Paciente),
                    LimpiarCsv(t.DniPaciente),
                    LimpiarCsv(t.Medico),
                    LimpiarCsv(t.Especialidad),
                    LimpiarCsv(t.Estado),
                    LimpiarCsv(t.Cobertura),
                    LimpiarCsv(t.Plan)
                );

                sb.AppendLine(linea);
            }

            return Encoding.UTF8.GetPreamble()
                .Concat(Encoding.UTF8.GetBytes(sb.ToString()))
                .ToArray();
        }

        public static byte[] GenerarReporteTratamientosCsv(List<ReporteTratamientosDto> lista)
        {
            if (lista == null || lista.Count == 0)
                throw new ArgumentException("No hay datos de tratamientos para exportar.");

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Tratamiento;Especialidad;Estado;Costo Base;Cantidad Realizados;Ingresos Estimados");

            foreach (var t in lista)
            {
                string linea = string.Join(";",
                    LimpiarCsv(t.Nombre),
                    LimpiarCsv(t.Especialidad),
                    LimpiarCsv(t.Estado),
                    t.CostoBase.ToString("F2"),
                    t.CantidadRealizados.ToString(),
                    t.IngresosEstimados.ToString("F2")
                );

                sb.AppendLine(linea);
            }

            return Encoding.UTF8.GetPreamble()
                .Concat(Encoding.UTF8.GetBytes(sb.ToString()))
                .ToArray();
        }
        public static byte[] GenerarReporteCoberturasCsv(List<ReporteCoberturasDto> lista)
        {
            if (lista == null || lista.Count == 0)
                throw new ArgumentException("No hay datos de coberturas para exportar.");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Obra Social;Estado;Cantidad Planes;Total Turnos;Pacientes Atendidos");

            foreach (var item in lista)
            {
                string linea = string.Join(";",
                    LimpiarCsv(item.Cobertura),
                    LimpiarCsv(item.Estado),
                    item.CantidadPlanes.ToString(),
                    item.TotalTurnos.ToString(),
                    item.PacientesAtendidos.ToString()
                );
                sb.AppendLine(linea);
            }
            return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
        }

        public static byte[] GenerarReportePlanesCsv(List<ReportePlanesDto> lista)
        {
            if (lista == null || lista.Count == 0)
                throw new ArgumentException("No hay datos de planes para exportar.");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Obra Social;Plan;Estado;Porcentaje Cobertura;Total Turnos");

            foreach (var item in lista)
            {
                string linea = string.Join(";",
                    LimpiarCsv(item.Cobertura),
                    LimpiarCsv(item.Plan),
                    LimpiarCsv(item.Estado),
                    item.PorcentajeCubierto.ToString("N0") + "%",
                    item.TotalTurnos.ToString()
                );
                sb.AppendLine(linea);
            }
            return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
        }
    }
}
