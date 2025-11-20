using iTextSharp.text;
using iTextSharp.text.pdf;
using SGTO.Comun.DTOs;
using SGTO.Negocio.DTOs.Pacientes;
using System;
using System.Collections.Generic;
using System.IO;

// fuente: https://vbpuntonet.blogspot.com/2019/01/genera-pdf-de-web-form-con-itextsharp.html

namespace SGTO.Negocio.Servicios.Exportacion
{
    public static class GeneradorPdf
    {
        private static readonly Font FuenteTitulo = new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD, BaseColor.DARK_GRAY);
        private static readonly Font FuenteSubtitulo = new Font(Font.FontFamily.HELVETICA, 12, Font.BOLD, BaseColor.BLACK);
        private static readonly Font FuenteTexto = new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK);
        private static readonly Font FuenteTextoBold = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD, BaseColor.BLACK);
        private static readonly Font FuenteFecha = new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.GRAY);
        private static readonly Font FuenteSmall = new Font(Font.FontFamily.HELVETICA, 8, Font.ITALIC, BaseColor.GRAY);

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


        public static byte[] GenerarHistoriaClinicaPdf(PacienteDetalleDto paciente, 
            List<HistoriaClinicaResumenDto> historial, 
            string nombreClinica)
        {
            if (paciente == null) throw new ArgumentNullException(nameof(paciente));
            if (historial == null) historial = new List<HistoriaClinicaResumenDto>();

            // si el parámetro esta vacio, usamos un default
            string tituloClinica = !string.IsNullOrWhiteSpace(nombreClinica) ? nombreClinica : "CLÍNICA ODONTOLÓGICA";

            using (MemoryStream ms = new MemoryStream())
            {
                // margenes izquierdo, derecho, arriba y abajo
                Document doc = new Document(PageSize.A4, 50, 50, 60, 50);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                doc.Open();

                // generar encabezado
                PdfPTable headerTable = new PdfPTable(1);
                headerTable.WidthPercentage = 100;

          
                PdfPCell cellHeader = new PdfPCell(new Phrase(tituloClinica.ToUpper(), FuenteTitulo));
                cellHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                cellHeader.Border = Rectangle.NO_BORDER;
                headerTable.AddCell(cellHeader);

                PdfPCell cellSubHeader = new PdfPCell(new Phrase("Informe de Historia Clínica", FuenteSubtitulo));
                cellSubHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                cellSubHeader.Border = Rectangle.NO_BORDER;
                cellSubHeader.PaddingBottom = 20f;
                headerTable.AddCell(cellSubHeader);

                doc.Add(headerTable);

                // armamos la ficha del paciente
                PdfPTable fichaTable = new PdfPTable(2);
                fichaTable.WidthPercentage = 100;
                fichaTable.SetWidths(new float[] { 1, 1 });
                fichaTable.SpacingAfter = 20f;

                PdfPCell cellFichaContainer = new PdfPCell();
                cellFichaContainer.Colspan = 2;
                cellFichaContainer.BackgroundColor = new BaseColor(240, 240, 240);
                cellFichaContainer.BorderColor = BaseColor.LIGHT_GRAY;
                cellFichaContainer.Padding = 10f;

                PdfPTable innerFicha = new PdfPTable(2);
                innerFicha.WidthPercentage = 100;

                // fila 1
                innerFicha.AddCell(CrearCeldaDato("Paciente:", paciente.NombreCompleto));
                innerFicha.AddCell(CrearCeldaDato("DNI:", paciente.Dni));

                // fila 2
                innerFicha.AddCell(CrearCeldaDato("Fecha Nac.:", paciente.FechaNacimiento));
                string coberturaInfo = string.IsNullOrEmpty(paciente.Plan) ? paciente.Cobertura : $"{paciente.Cobertura} - {paciente.Plan}";
                innerFicha.AddCell(CrearCeldaDato("Cobertura:", coberturaInfo));

                // fila 3
                innerFicha.AddCell(CrearCeldaDato("Fecha Emisión:", DateTime.Now.ToString("dd/MM/yyyy HH:mm")));
                innerFicha.AddCell(CrearCeldaDato("", ""));

                cellFichaContainer.AddElement(innerFicha);
                fichaTable.AddCell(cellFichaContainer);

                doc.Add(fichaTable);

                // generar el historial en forma cronologica
                if (historial.Count == 0)
                {
                    doc.Add(new Paragraph("No se registran atenciones clínicas finalizadas para este paciente.", FuenteTexto));
                }
                else
                {
                    foreach (var registro in historial)
                    {
                        // para cada registro se crea una tabla para que sea un bloque visual
                        PdfPTable registroTable = new PdfPTable(1);
                        registroTable.WidthPercentage = 100;
                        registroTable.KeepTogether = true; // Evita que se corte a la mitad al cambiar de hoja
                        registroTable.SpacingAfter = 10f;

                        // bara de titulo con fecha y medico
                        PdfPCell cellBarra = new PdfPCell();
                        cellBarra.BackgroundColor = new BaseColor(230, 240, 255);
                        cellBarra.BorderColor = new BaseColor(200, 200, 200);
                        cellBarra.BorderWidthBottom = 0;
                        cellBarra.Padding = 6f;

                        // tabla para mostrar fecha a la izquierda y medico a la derecha
                        PdfPTable titleBar = new PdfPTable(2);
                        titleBar.WidthPercentage = 100;

                        PdfPCell cellFecha = new PdfPCell(new Phrase(registro.Fecha.ToString("dd/MM/yyyy"), FuenteTextoBold));
                        cellFecha.Border = Rectangle.NO_BORDER;

                        // medico a la derecha
                        PdfPCell cellProf = new PdfPCell(new Phrase($"Prof. {registro.Profesional} ({registro.Especialidad})", FuenteTexto));
                        cellProf.HorizontalAlignment = Element.ALIGN_RIGHT;
                        cellProf.Border = Rectangle.NO_BORDER;

                        titleBar.AddCell(cellFecha);
                        titleBar.AddCell(cellProf);

                        cellBarra.AddElement(titleBar);
                        registroTable.AddCell(cellBarra);

                        // tratamiento y diagnostico
                        PdfPCell cellBody = new PdfPCell();
                        cellBody.BorderColor = new BaseColor(200, 200, 200);
                        cellBody.BorderWidthTop = 0; 
                        cellBody.Padding = 8f;

                        // Tratamiento
                        Paragraph pTratamiento = new Paragraph();
                        pTratamiento.Add(new Chunk("Tratamiento: ", FuenteTextoBold));
                        pTratamiento.Add(new Chunk(registro.Tratamiento, FuenteTexto));
                        pTratamiento.SpacingAfter = 4f;
                        cellBody.AddElement(pTratamiento);

                        // Diagnóstico
                        Paragraph pDiagnostico = new Paragraph();
                        pDiagnostico.Add(new Chunk("Diagnóstico/Evolución: ", FuenteTextoBold));
                        pDiagnostico.Add(new Chunk(registro.Diagnostico, FuenteTexto));
                        cellBody.AddElement(pDiagnostico);

                        registroTable.AddCell(cellBody);
                        doc.Add(registroTable);
                    }
                }

                // pie de pagina
                Paragraph footer = new Paragraph($"Documento generado electrónicamente por Sistema {tituloClinica}.", FuenteSmall);
                footer.Alignment = Element.ALIGN_CENTER;
                footer.SpacingBefore = 30f;
                doc.Add(footer);

                doc.Close();
                return ms.ToArray();
            }
        }

        private static PdfPCell CrearCeldaDato(string label, string valor)
        {
            Phrase frase = new Phrase();
            if (!string.IsNullOrEmpty(label))
                frase.Add(new Chunk(label + " ", FuenteTextoBold));

            frase.Add(new Chunk(valor, FuenteTexto));

            PdfPCell cell = new PdfPCell(frase);
            cell.Border = Rectangle.NO_BORDER;
            cell.PaddingBottom = 4f;
            return cell;
        }


    }
}
