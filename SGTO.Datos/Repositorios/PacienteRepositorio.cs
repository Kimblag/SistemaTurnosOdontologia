using SGTO.Datos.Infraestructura;
using SGTO.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGTO.Datos.Repositorios
{
    public class PacienteRepositorio
    {

        public List<Paciente> Listar(string estado = null)
        {
            List<Paciente> pacientes = new List<Paciente>();

            using (ConexionDBFactory datos = new ConexionDBFactory())
            {
                string query = @"SELECT PCTE.IdPaciente,
                                   PCTE.Apellido AS ApellidoPaciente,
                                   PCTE.Nombre AS NombrePaciente,
                                   PCTE.NumeroDocumento,
                                   PCTE.Genero,
                                   PCTE.Telefono,
                                   PCTE.Email,
                                   PCTE.Estado AS EstadoPaciente,
                                   C.IdCobertura,
                                   C.Nombre AS NombreCobertura,
                                   C.Descripcion AS DescripcionCobertura,
                                   C.Estado AS EstadoCobertura,
                                   P.IdPlan,
                                   P.Nombre AS NombrePlan,
                                   P.Descripcion AS DescripcionPlan,
                                   P.PorcentajeCobertura,
                                   P.Estado AS EstadoPlan
                            FROM Paciente PCTE
                            INNER JOIN Cobertura C ON PCTE.IdCobertura = C.IdCobertura
                            LEFT JOIN [Plan] P ON PCTE.IdPlan = P.IdPlan
                            {{WHERE}}
                            ORDER BY PCTE.Apellido, PCTE.Nombre";

                query = estado != null
                      ? query.Replace("{{WHERE}}", $" WHERE UPPER(PCTE.Estado) = UPPER('{estado[0]}')")
                      : query.Replace("{{WHERE}}", " ");


                try
                {
                    datos.DefinirConsulta(query);

                    using (SqlDataReader lector = datos.EjecutarConsulta())
                    {
                        while (lector.Read())
                        {

                        }
                    }

                    return pacientes;

                }
                catch (Exception)
                {

                    throw;
                }

            }





        }

    }
}
