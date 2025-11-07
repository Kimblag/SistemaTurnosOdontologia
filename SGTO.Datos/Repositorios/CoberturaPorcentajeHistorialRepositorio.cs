using SGTO.Datos.Infraestructura;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGTO.Datos.Repositorios
{
    public class CoberturaPorcentajeHistorialRepositorio
    {
        public decimal? ObtenerPorcentajeActivo(int idCobertura, ConexionDBFactory datos)
        {
            string query = @"SELECT TOP 1 PorcentajeCobertura
                             FROM CoberturaPorcentajeHistorial
                             WHERE IdCobertura = @IdCobertura AND Estado = 'A'
                             ORDER BY FechaInicio DESC";

            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@IdCobertura", idCobertura);

            using (SqlDataReader lector = datos.EjecutarConsulta())
            {
                if (lector.Read() && !lector.IsDBNull(0))
                    return lector.GetDecimal(0);
            }

            return null;
        }

        public void CerrarPorcentajeActivo(int idCobertura, ConexionDBFactory datos)
        {
            string query = @"UPDATE CoberturaPorcentajeHistorial
                             SET Estado = 'I', FechaFin = GETDATE()
                             WHERE IdCobertura = @IdCobertura AND Estado = 'A'";

            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@IdCobertura", idCobertura);
            datos.EjecutarAccion();
        }

        public void InsertarNuevoPorcentaje(int idCobertura, decimal porcentaje, string motivo, ConexionDBFactory datos)
        {
            string query = @"INSERT INTO CoberturaPorcentajeHistorial
                             (IdCobertura, PorcentajeCobertura, FechaInicio, Estado, MotivoCambio)
                             VALUES (@IdCobertura, @Porcentaje, GETDATE(), 'A', @Motivo)";

            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@IdCobertura", idCobertura);
            datos.EstablecerParametros("@Porcentaje", porcentaje);
            datos.EstablecerParametros("@Motivo", (object)motivo ?? "Actualización desde UI");
            datos.EjecutarAccion();
        }
    }
}
