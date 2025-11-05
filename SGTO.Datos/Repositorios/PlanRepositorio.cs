using SGTO.Datos.Infraestructura;
using SGTO.Datos.Mappers;
using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
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

        public void ActualizarEstadoPorCobertura(int idCobertura, EstadoEntidad nuevoEstado, ConexionDBFactory datos)
        {
            string query = @"UPDATE [Plan]
                     SET Estado = @Estado
                     WHERE IdCobertura = @IdCobertura";

            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@Estado", EnumeracionMapper.ObtenerChar(nuevoEstado));
            datos.EstablecerParametros("@IdCobertura", idCobertura);
            datos.EjecutarAccion();
        }

        public void Crear(Plan nuevoPlan, ConexionDBFactory datos)
        {
            string query = @"INSERT INTO [Plan] (Nombre, Descripcion, PorcentajeCobertura, IdCobertura, Estado)
                                VALUES (@Nombre, @Descripcion, @PorcentajeCobertura, @IdCobertura, @Estado)";
            datos.LimpiarParametros();
            datos.DefinirConsulta(query);
            datos.EstablecerParametros("@Nombre", nuevoPlan.Nombre);
            datos.EstablecerParametros("@Descripcion", nuevoPlan.Descripcion);
            datos.EstablecerParametros("@PorcentajeCobertura", nuevoPlan.PorcentajeCobertura);
            datos.EstablecerParametros("@IdCobertura", nuevoPlan.Cobertura.IdCobertura);
            datos.EstablecerParametros("@Estado", EnumeracionMapper.ObtenerChar(nuevoPlan.Estado));
            datos.EjecutarAccion();
        }


        public void Crear(List<Plan> planes, ConexionDBFactory datos)
        {
            // método para crear planes a partir de una lista.

            string query = @"INSERT INTO [Plan] (Nombre, Descripcion, PorcentajeCobertura, IdCobertura, Estado)
                     VALUES (@Nombre, @Descripcion, @PorcentajeCobertura, @IdCobertura, @Estado)";

            foreach (Plan nuevoPlan in planes)
            {
                datos.LimpiarParametros();
                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@Nombre", nuevoPlan.Nombre);
                datos.EstablecerParametros("@Descripcion", nuevoPlan.Descripcion);
                datos.EstablecerParametros("@PorcentajeCobertura", nuevoPlan.PorcentajeCobertura);
                datos.EstablecerParametros("@IdCobertura", nuevoPlan.Cobertura.IdCobertura);
                datos.EstablecerParametros("@Estado", EnumeracionMapper.ObtenerChar(nuevoPlan.Estado));
                datos.EjecutarAccion();
            }
        }



    }
}
