using SGTO.Datos.Repositorios;
using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Negocio.DTOs.Seguridad;
using SGTO.Negocio.Excepciones;
using System;


namespace SGTO.Negocio.Seguridad
{
    public class ServicioAutenticacion
    {

        private readonly UsuarioRepositorio _repositorioUsuario;
        private readonly RolRepositorio _repositorioRol;
        private readonly MedicoRepositorio _repositorioMedico;

        public ServicioAutenticacion()
        {
            _repositorioUsuario = new UsuarioRepositorio();
            _repositorioRol = new RolRepositorio();
            _repositorioMedico = new MedicoRepositorio();
        }

        public UsuarioSesionDto Autenticar(string credencial, string password)
        {
            // validar que las credenciales no sean inválidas
            if (string.IsNullOrWhiteSpace(credencial) || string.IsNullOrWhiteSpace(password))
                throw new ExcepcionAutenticacion("Debe ingresar usuario y contraseña.");

            Usuario usuario = _repositorioUsuario.ObtenerPorCredencial(credencial);

            // validar que el usuario exista y que la pass sea correcta
            if (usuario == null || !PasswordHasher.Verify(password, usuario.PasswordHash))
                throw new ExcepcionAutenticacion("Credenciales inválidas. Verifique usuario y contraseña.");

            // un usuario inactivo no puede ingresar al sistema!
            if (usuario.Estado != EstadoEntidad.Activo)
                throw new ExcepcionAutenticacion("El usuario se encuentra inactivo. Contacte al administrador.");

            UsuarioSesionDto sesion = new UsuarioSesionDto()
            {
                IdUsuario = usuario.IdUsuario,
                NombreUsuario = usuario.NombreUsuario,
                Email = usuario.Email.Valor,
                NombreCompleto = usuario.NombreCompleto(),
                IdRol = usuario.Rol.IdRol,
                NombreRol = usuario.Rol.Nombre,
                EsAdmin = usuario.Rol.Nombre.Equals("Administrador", StringComparison.InvariantCultureIgnoreCase)
            };

            // Si el usuario es médico, buscar su info
            if(usuario.Rol.Nombre.Equals("Médico", StringComparison.InvariantCultureIgnoreCase))
            {
                Medico medico = _repositorioMedico.ObtenerPorUsuarioId(usuario.IdUsuario);
                if (medico != null)
                    sesion.IdMedico = medico.IdMedico;
            }

            // si todo esta ok cargamos entonces los permisos del usuario
            Rol rol = _repositorioRol.ObtenerPorId(usuario.Rol.IdRol);

            if (rol != null && rol.Permisos != null)
            {
                foreach (Permiso permiso in rol.Permisos)
                {
                    sesion.Permisos.Add(permiso.Clave());
                }
            }

            return sesion;
        }

    }
}
