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
        public static byte[] GenerarReporteMedicosPdf(List<ReporteMedicosDto> lista)
        {
            if (lista == null || lista.Count == 0)
                throw new ArgumentException("No hay datos de médicos para exportar.");

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var titulo = new Paragraph("Reporte de Médicos", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD))
                {
                    Alignment = Element.ALIGN_CENTER
                };
                doc.Add(titulo);
                doc.Add(new Paragraph($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}", new Font(Font.FontFamily.HELVETICA, 10, Font.ITALIC)));
                doc.Add(new Paragraph(" "));

                PdfPTable table = new PdfPTable(7);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 2, 4, 3, 2, 2, 2, 2 });

                string[] headers = { "Matrícula", "Nombre", "Especialidad", "Estado", "Turnos", "Pacientes", "Ult. Atenc." };

                foreach (string h in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(h, new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD))) 
                    {
                        BackgroundColor = new BaseColor(240, 240, 240),
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 5
                    };
                    table.AddCell(cell);
                }

                foreach (var m in lista)
                {
                    table.AddCell(new Phrase(m.Matricula ?? "-", FuenteSmall));
                    table.AddCell(new Phrase(m.NombreCompleto ?? "-", FuenteTexto));
                    table.AddCell(new Phrase(m.Especialidad ?? "Sin esp.", FuenteSmall));

                    PdfPCell celdaEstado = new PdfPCell(new Phrase(m.Estado ?? "-", FuenteSmall));
                    celdaEstado.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(celdaEstado);

                    PdfPCell celdaTurnos = new PdfPCell(new Phrase(m.TotalTurnos.ToString(), FuenteTexto));
                    celdaTurnos.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(celdaTurnos);

                    PdfPCell celdaPacientes = new PdfPCell(new Phrase(m.PacientesAtendidos.ToString(), FuenteTexto));
                    celdaPacientes.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(celdaPacientes);

                    table.AddCell(new Phrase(m.UltimoTurno?.ToString("dd/MM/yyyy") ?? "-", FuenteSmall));
                }

                doc.Add(table);
                doc.Close();

                return ms.ToArray();
            }
        }


        public static byte[] GenerarReporteTurnosPdf(List<ReporteTurnosDto> lista)
        {
            if (lista == null || lista.Count == 0)
                throw new ArgumentException("No hay datos de turnos para exportar.");

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 30, 30, 40, 40);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var titulo = new Paragraph("Reporte de Turnos", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD))
                {
                    Alignment = Element.ALIGN_CENTER
                };
                doc.Add(titulo);
                doc.Add(new Paragraph($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}", new Font(Font.FontFamily.HELVETICA, 10, Font.ITALIC)));
                doc.Add(new Paragraph(" "));

                PdfPTable table = new PdfPTable(7);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 2, 1, 3, 3, 3, 2, 2 });

                string[] headers = { "Fecha", "Hora", "Paciente", "Médico", "Especialidad", "Estado", "Cobertura" };

                foreach (string h in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(h, new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD)))
                    {
                        BackgroundColor = new BaseColor(240, 240, 240),
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 5
                    };
                    table.AddCell(cell);
                }

                foreach (var t in lista)
                {
                    table.AddCell(new Phrase(t.Fecha.ToString("dd/MM/yyyy"), FuenteSmall));
                    table.AddCell(new Phrase(t.Hora, FuenteSmall));
                    table.AddCell(new Phrase(t.Paciente, FuenteTexto));
                    table.AddCell(new Phrase(t.Medico, FuenteTexto));
                    table.AddCell(new Phrase(t.Especialidad, FuenteSmall));

                    PdfPCell cellEstado = new PdfPCell(new Phrase(t.Estado, FuenteSmall));
                    cellEstado.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cellEstado);

                    table.AddCell(new Phrase(t.Cobertura, FuenteSmall));
                }

                doc.Add(table);
                doc.Close();

                return ms.ToArray();
            }
        }

        public static byte[] GenerarReporteTratamientosPdf(List<ReporteTratamientosDto> lista)
        {
            if (lista == null || lista.Count == 0)
                throw new ArgumentException("No hay datos de tratamientos para exportar.");

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var titulo = new Paragraph("Reporte de Tratamientos", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD))
                {
                    Alignment = Element.ALIGN_CENTER
                };
                doc.Add(titulo);
                doc.Add(new Paragraph($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}", new Font(Font.FontFamily.HELVETICA, 10, Font.ITALIC)));
                doc.Add(new Paragraph(" "));

                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 4, 3, 2, 2, 2, 2 });

                string[] headers = { "Tratamiento", "Especialidad", "Estado", "Costo Base", "Cant. Realiz.", "Ingresos Est." };

                foreach (string h in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(h, new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD)))
                    {
                        BackgroundColor = new BaseColor(240, 240, 240),
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 5
                    };
                    table.AddCell(cell);
                }

                foreach (var t in lista)
                {
                    table.AddCell(new Phrase(t.Nombre ?? "-", FuenteTexto));
                    table.AddCell(new Phrase(t.Especialidad ?? "-", FuenteSmall));

                    PdfPCell celdaEstado = new PdfPCell(new Phrase(t.Estado ?? "-", FuenteSmall));
                    celdaEstado.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(celdaEstado);

                    PdfPCell celdaCosto = new PdfPCell(new Phrase(t.CostoBase.ToString("C"), FuenteTexto));
                    celdaCosto.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(celdaCosto);

                    PdfPCell celdaCant = new PdfPCell(new Phrase(t.CantidadRealizados.ToString(), FuenteTexto));
                    celdaCant.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(celdaCant);

                    PdfPCell celdaTotal = new PdfPCell(new Phrase(t.IngresosEstimados.ToString("C"), FuenteTexto));
                    celdaTotal.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(celdaTotal);
                }

                doc.Add(table);
                doc.Close();

                return ms.ToArray();
            }
        }
        public static byte[] GenerarReporteCoberturasPdf(List<ReporteCoberturasDto> lista)
        {
            if (lista == null || lista.Count == 0)
                throw new ArgumentException("No hay datos de coberturas para exportar.");

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var titulo = new Paragraph("Reporte de Obras Sociales", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD))
                {
                    Alignment = Element.ALIGN_CENTER
                };
                doc.Add(titulo);
                doc.Add(new Paragraph($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}", new Font(Font.FontFamily.HELVETICA, 10, Font.ITALIC)));
                doc.Add(new Paragraph(" "));

                PdfPTable table = new PdfPTable(5);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 4, 2, 2, 2, 2 });

                string[] headers = { "Obra Social", "Estado", "Planes", "Turnos", "Pacientes" };
                foreach (string h in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(h, new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD)))
                    {
                        BackgroundColor = new BaseColor(240, 240, 240),
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 5
                    };
                    table.AddCell(cell);
                }

                foreach (var item in lista)
                {
                    table.AddCell(new Phrase(item.Cobertura ?? "-", FuenteTexto));

                    PdfPCell cellEstado = new PdfPCell(new Phrase(item.Estado, FuenteSmall));
                    cellEstado.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cellEstado);

                    PdfPCell cellPlanes = new PdfPCell(new Phrase(item.CantidadPlanes.ToString(), FuenteTexto));
                    cellPlanes.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cellPlanes);

                    PdfPCell cellTurnos = new PdfPCell(new Phrase(item.TotalTurnos.ToString(), FuenteTexto));
                    cellTurnos.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cellTurnos);

                    PdfPCell cellPacientes = new PdfPCell(new Phrase(item.PacientesAtendidos.ToString(), FuenteTexto));
                    cellPacientes.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cellPacientes);
                }

                doc.Add(table);
                doc.Close();
                return ms.ToArray();
            }
        }

        public static byte[] GenerarReportePlanesPdf(List<ReportePlanesDto> lista)
        {
            if (lista == null || lista.Count == 0)
                throw new ArgumentException("No hay datos de planes para exportar.");

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                var titulo = new Paragraph("Reporte de Planes", new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD))
                {
                    Alignment = Element.ALIGN_CENTER
                };
                doc.Add(titulo);
                doc.Add(new Paragraph($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}", new Font(Font.FontFamily.HELVETICA, 10, Font.ITALIC)));
                doc.Add(new Paragraph(" "));

                // Tabla 5 columnas
                PdfPTable table = new PdfPTable(5);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 3, 3, 2, 2, 2 });

                string[] headers = { "Obra Social", "Plan", "Estado", "% Cobertura", "Turnos" };
                foreach (string h in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(h, new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD)))
                    {
                        BackgroundColor = new BaseColor(240, 240, 240),
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        Padding = 5
                    };
                    table.AddCell(cell);
                }

                foreach (var item in lista)
                {
                    table.AddCell(new Phrase(item.Cobertura ?? "-", FuenteTexto));
                    table.AddCell(new Phrase(item.Plan ?? "-", FuenteTexto));

                    PdfPCell cellEstado = new PdfPCell(new Phrase(item.Estado, FuenteSmall));
                    cellEstado.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cellEstado);

                    PdfPCell cellPorc = new PdfPCell(new Phrase(item.PorcentajeCubierto.ToString("N0") + "%", FuenteTexto));
                    cellPorc.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table.AddCell(cellPorc);

                    PdfPCell cellTurnos = new PdfPCell(new Phrase(item.TotalTurnos.ToString(), FuenteTexto));
                    cellTurnos.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cellTurnos);
                }

                doc.Add(table);
                doc.Close();
                return ms.ToArray();
            }
        }

    }
}
