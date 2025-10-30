using SGTO.Datos.Infraestructura;
using SGTO.Datos.Mappers;
using SGTO.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace SGTO.Datos.Repositorios
{
    public class CoberturaRepositorio
    {
        public List<Cobertura> Listar()
        {
            List<Cobertura> coberturas = new List<Cobertura>();

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {

                try
                {
                    datos.DefinirConsulta(@"SELECT IdCobertura, Nombre, Descripcion, Estado 
                                                FROM Coberturas");

                    // usamos using para aplicar buenas prácticas y evitar conexiones abiertas 
                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {

                        while (lector.Read())
                        {
                            Cobertura cobertura = CoberturaMapper.MapearAEntidad(lector);
                            coberturas.Add(cobertura);
                        }
                    }
                    return coberturas;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
