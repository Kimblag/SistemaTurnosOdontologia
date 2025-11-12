using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using SGTO.Comun.DTOs;

// fuente: https://vbpuntonet.blogspot.com/2019/01/genera-pdf-de-web-form-con-itextsharp.html

namespace SGTO.Negocio.Servicios.Exportacion
{
    public static class GeneradorPdf
    {
        public static byte[] GenerarReportePacientesPdf(List<ReportePacientesDto> lista)
        {
            if (lista == null || lista.Count == 0)
                throw new ArgumentException("No hay datos para exportar al PDF.");

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var titulo = new Paragraph("Reporte de Pacientes", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD))
                {
                    Alignment = Element.ALIGN_CENTER
                };
                doc.Add(titulo);
                doc.Add(new Paragraph($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}", new Font(Font.FontFamily.HELVETICA, 10, Font.ITALIC)));
                doc.Add(new Paragraph(" "));

                PdfPTable table = new PdfPTable(7);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 2, 2, 2, 2, 1, 2, 2 });

                string[] headers = { "Nombre", "DNI", "Cobertura", "Plan", "Turnos", "Última Atención", "Médico Frecuente" };
                foreach (string h in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(h, new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)))
                    {
                        BackgroundColor = new BaseColor(240, 240, 240),
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    table.AddCell(cell);
                }

                foreach (var p in lista)
                {
                    table.AddCell(new Phrase(p.NombreCompleto ?? "-"));
                    table.AddCell(new Phrase(p.NumeroDocumento ?? "-"));
                    table.AddCell(new Phrase(p.Cobertura ?? "-"));
                    table.AddCell(new Phrase(p.Plan ?? "-"));
                    table.AddCell(new Phrase(p.TotalTurnos.ToString()));
                    table.AddCell(new Phrase(p.UltimaAtencion?.ToString("dd/MM/yyyy") ?? "-"));
                    table.AddCell(new Phrase(p.MedicoFrecuente ?? "-"));
                }

                doc.Add(table);
                doc.Close();

                return ms.ToArray();
            }
        }
    }
}
