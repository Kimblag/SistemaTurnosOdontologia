using SGTO.Datos.Repositorios;
using SGTO.Negocio.DTOs.Pacientes;
using SGTO.Negocio.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGTO.Negocio.Servicios
{
    public class PacienteService
    {
        private readonly PacienteRepositorio _repositorioPaciente;

        public PacienteService()
        {
            _repositorioPaciente = new PacienteRepositorio();
        }


        public List<PacienteListadoDto> ListarPacientes(string estado = null)
        {
            try
            {
                return PacienteMapper.MapearAListado(_repositorioPaciente.Listar(estado));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PacienteEdicionDto ObtenerPorId(int idPaciente)
        {
            try
            {
                return PacienteMapper.MapearAEdicionDto(_repositorioPaciente.ObtenerPorId(idPaciente));
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}
