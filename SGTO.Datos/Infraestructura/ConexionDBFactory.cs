using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGTO.Datos.Infraestructura
{
    public class ConexionDBFactory : IDisposable
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlTransaction transaccion;

        public ConexionDBFactory()
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=SistemaOdontologico; integrated security=true");
            comando = new SqlCommand();
        }

        public void DefinirConsulta(string consultaSql)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consultaSql;
        }

        public SqlDataReader EjecutarConsulta()
        {
            comando.Connection = conexion;

            try
            {
                if (conexion.State == System.Data.ConnectionState.Closed) conexion.Open();

                return comando.ExecuteReader();
            }
            catch (Exception)
            {
                Dispose(); // aprovehcamos la interface IDisposable que nos brinda seguridad con las conexiones
                throw;
            }
        }

        public void EjecutarAccion()
        {
            comando.Connection = conexion;

            try
            {
                if (conexion.State == System.Data.ConnectionState.Closed) conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int EjecutarAccionEscalar()
        {
            comando.Connection = conexion;
            try
            {
                if (conexion.State == System.Data.ConnectionState.Closed) conexion.Open();
                object id = comando.ExecuteScalar();
                if (id == null || id == DBNull.Value)
                    return 0;

                return Convert.ToInt32(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EstablecerParametros(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }

        public void LimpiarParametros()
        {
            comando.Parameters.Clear();
        }

        public void IniciarTransaccion()
        {
            try
            {
                if (conexion.State == System.Data.ConnectionState.Closed)
                    conexion.Open();
                transaccion = conexion.BeginTransaction();
                comando.Transaction = transaccion;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ConfirmarTransaccion()
        {
            try
            {
                transaccion.Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RollbackTransaccion()
        {
            try
            {
                transaccion?.Rollback();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            // metodo de la interfaz IDisposable
            // cerrar la conexion solo si existe y esta abierta
            if (conexion != null && conexion.State == System.Data.ConnectionState.Open)
                conexion.Close();

            // liberar recursos usados durante la conexion
            conexion?.Dispose();
            comando?.Dispose();
            comando = null;
        }
    }
}
