using SGTO.Datos.Infraestructura;
using SGTO.Datos.Mappers;
using SGTO.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SGTO.Datos.Repositorios
{
    public class ParametroSistemaRepositorio
    {

        public List<ParametroSistema> Listar()
        {
            List<ParametroSistema> parametros = new List<ParametroSistema>();

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"SELECT IdParametroSistema, Nombre, Valor, Descripcion 
                                 FROM ParametroSistema
                                 ORDER BY Nombre ASC";
                try
                {
                    datos.DefinirConsulta(query);
                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        while (lector.Read())
                        {
                            parametros.Add(ParametroSistemaMapper.MapearAEntidad(lector));
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return parametros;
        }


        public void ActualizarValor(string nombre, string nuevoValor, ConexionDBFactory datos)
        {
            string query = @"UPDATE ParametroSistema
                             SET Valor = @Valor
                             WHERE Nombre = @Nombre";

            try
            {
                datos.LimpiarParametros();
                datos.DefinirConsulta(query);
                datos.EstablecerParametros("@Valor", nuevoValor);
                datos.EstablecerParametros("@Nombre", nombre);
                datos.EjecutarAccion();
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
