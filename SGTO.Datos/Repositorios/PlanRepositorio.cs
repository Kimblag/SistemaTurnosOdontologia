using SGTO.Datos.Infraestructura;
using System;
using System.Collections.Generic;


namespace SGTO.Datos.Repositorios
{
    public class PlanRepositorio
    {

        public void DarDeBaja(int idCobertura, char estado, ConexionDBFactory datos)
        {
            string query = @"UPDATE [Plan] 
                                    SET Estado = @Estado
                                WHERE IdCobertura = @IdCobertura";

            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@Estado", estado);
            datos.EstablecerParametros("@IdCobertura", idCobertura);
            datos.EjecutarAccion();
        }

        public void ActualizarEstadoPorCobertura(int idCobertura, char nuevoEstado, ConexionDBFactory datos)
        {
            string query = @"UPDATE [Plan]
                     SET Estado = @Estado
                     WHERE IdCobertura = @IdCobertura";

            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@Estado", nuevoEstado);
            datos.EstablecerParametros("@IdCobertura", idCobertura);
            datos.EjecutarAccion();
        }


    }
}
