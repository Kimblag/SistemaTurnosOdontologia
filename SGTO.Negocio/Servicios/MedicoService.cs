using SGTO.Datos.Repositorios;

namespace SGTO.Negocio.Servicios
{
    public class MedicoService
    {

        private readonly MedicoRepositorio _repositorioMedico;

        public MedicoService()
        {
            _repositorioMedico = new MedicoRepositorio();
        }


    }
}
