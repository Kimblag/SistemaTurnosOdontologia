using SGTO.Comun.Validacion;
using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Negocio.DTOs.Roles;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Controles.Configuracion.Roles
{
    public partial class RolesForm : System.Web.UI.UserControl
    {

        private readonly RolService _servicioRol = new RolService();

        public bool ModoEdicion { get; set; } = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            int idRol = ValidarModoEdicion();

            if (!IsPostBack)
            {
                if (!ModoEdicion)
                {
                    ddlEstado.SelectedValue = "Activo";
                    ddlEstado.Enabled = false;

                    MarcarPermisosVerPorDefecto();
                }
                else
                {
                    CargarDetalleRol(idRol);
                }

                ModalHelper.MostrarModalDesdeSession(
                    this.Page,
                    "RolMensajeTitulo",
                    "RolMensajeDesc",
                    "/Pages/Configuracion/Roles/Index"
                );
            }
        }

        private int ExtraerIdRol()
        {
            string idStr = Request.QueryString["id-rol"] ?? string.Empty;
            int id;
            if (int.TryParse(idStr, out id))
                return id;
            return 0;
        }

        private int ValidarModoEdicion()
        {
            int idRol = ExtraerIdRol();
            if (idRol != 0)
            {
                ModoEdicion = true;
            }
            return idRol;
        }


        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect($"~/Pages/Configuracion/Roles/Index", false);
        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarCampos();

                if (ModoEdicion)
                    ModificarRol();
                else
                    CrearNuevoRol();
            }
            catch (ArgumentException ex)
            {
                MostrarModalError("Dato inválido", ex.Message);
            }
            catch (ExcepcionReglaNegocio ex)
            {
                MostrarModalError("Operación no permitida", ex.Message);
            }
            catch (Exception ex)
            {
                MostrarModalError("Error inesperado", "Ocurrió un error al guardar el rol: " + ex.Message);
            }
        }

        private void ValidarCampos()
        {
            string nombre = txtNombre.Text.Trim();
            string descripcion = txtDescripcion.Text.Trim();

            if (!ValidadorCampos.EsTextoObligatorio(nombre))
                throw new ArgumentException("El nombre del rol es obligatorio.");

            if (!ValidadorCampos.EsSoloLetrasYEspacios(nombre))
                throw new ArgumentException("El nombre solo puede contener letras y espacios.");

            if (!ValidadorCampos.TieneLongitudMaxima(nombre, 50))
                throw new ArgumentException("El nombre no puede superar los 50 caracteres.");

            if (!string.IsNullOrWhiteSpace(descripcion))
            {
                if (!ValidadorCampos.TieneLongitudMinima(descripcion, 10))
                    throw new ArgumentException("La descripción debe tener al menos 10 caracteres si se completa.");
                if (!ValidadorCampos.TieneLongitudMaxima(descripcion, 200))
                    throw new ArgumentException("La descripción no puede superar los 200 caracteres.");
            }
        }

        private void CrearNuevoRol()
        {
            List<Permiso> permisosDisponibles = _servicioRol.ListarPermisos();
            List<int> idsSeleccionados = ObtenerIdsPermisosSeleccionados(permisosDisponibles);

            RolCrearDto dto = new RolCrearDto()
            {
                Nombre = txtNombre.Text.Trim(),
                Descripcion = txtDescripcion.Text.Trim(),
                Estado = "Activo",
                IdPermisos = idsSeleccionados
            };

            _servicioRol.Crear(dto);

            MensajeUiHelper.SetearYMostrar(
                this.Page,
                "Rol creado correctamente",
                "El rol se ha registrado con éxito.",
                "Resultado",
                VirtualPathUtility.ToAbsolute("~/Pages/Configuracion/Roles/Index.aspx"),
                "abrirModalResultado"
            );
        }

        private void ModificarRol()
        {
            int idRol = ExtraerIdRol();
            List<Permiso> permisosDisponibles = _servicioRol.ListarPermisos();
            List<int> idsSeleccionados = ObtenerIdsPermisosSeleccionados(permisosDisponibles);

            RolDetalleDto dto = new RolDetalleDto()
            {
                IdRol = idRol,
                Nombre = txtNombre.Text.Trim(),
                Descripcion = txtDescripcion.Text.Trim(),
                Estado = ddlEstado.SelectedValue,
                IdPermisos = idsSeleccionados
            };

            _servicioRol.Modificar(dto);

            MensajeUiHelper.SetearYMostrar(
                this.Page,
                "Rol actualizado",
                "Los datos del rol se modificaron correctamente.",
                "Resultado",
                VirtualPathUtility.ToAbsolute("~/Pages/Configuracion/Roles/Index"),
                "abrirModalResultado"
            );
        }

        private List<int> ObtenerIdsPermisosSeleccionados(List<Permiso> permisosDisponibles)
        {
            List<int> ids = new List<int>();
            Array modulos = Enum.GetValues(typeof(Modulo));
            Array acciones = Enum.GetValues(typeof(TipoAccion));

            for (int i = 0; i < modulos.Length; i++)
            {
                Modulo modulo = (Modulo)modulos.GetValue(i);
                string nombreModulo = modulo.ToString();

                for (int j = 0; j < acciones.Length; j++)
                {
                    TipoAccion accion = (TipoAccion)acciones.GetValue(j);
                    string nombreAccion = accion.ToString();

                    string idControl = "chk" + nombreModulo + nombreAccion;
                    CheckBox chk = this.FindControl(idControl) as CheckBox;

                    if (chk != null && chk.Checked)
                    {
                        int idPermiso = BuscarIdPermiso(permisosDisponibles, modulo, accion);
                        if (idPermiso != 0)
                            ids.Add(idPermiso);
                    }
                }
            }
            return ids;
        }

        private int BuscarIdPermiso(List<Permiso> permisos, Modulo modulo, TipoAccion accion)
        {
            for (int i = 0; i < permisos.Count; i++)
            {
                if (permisos[i].Modulo == modulo && permisos[i].Accion == accion)
                    return permisos[i].IdPermiso;
            }
            return 0;
        }

        private void MarcarPermisosVerPorDefecto()
        {
            Array modulos = Enum.GetValues(typeof(Modulo));

            for (int i = 0; i < modulos.Length; i++)
            {
                string idControl = "chk" + ((Modulo)modulos.GetValue(i)).ToString() + "Ver";
                CheckBox chk = this.FindControl(idControl) as CheckBox;
                if (chk != null)
                    chk.Checked = true;
            }
        }

        private void MostrarModalError(string titulo, string mensaje)
        {
            MensajeUiHelper.SetearYMostrar(
                this.Page,
                titulo,
                mensaje,
                "Resultado",
                null,
                "abrirModalResultado"
            );
        }

        private void CargarDetalleRol(int idRol)
        {
            RolDetalleDto rol = _servicioRol.ObtenerPorId(idRol);
            if (rol == null)
            {
                MostrarModalError("No encontrado", "No se encontró el rol solicitado.");
                return;
            }

            txtNombre.Text = rol.Nombre;
            txtDescripcion.Text = rol.Descripcion;
            ddlEstado.SelectedValue = rol.Estado;

            // marcar permisos
            List<Permiso> permisosDisponibles = _servicioRol.ListarPermisos();

            for (int i = 0; i < rol.IdPermisos.Count; i++)
            {
                int idPermiso = rol.IdPermisos[i];
                Permiso permiso = permisosDisponibles.Find(p => p.IdPermiso == idPermiso);
                if (permiso != null)
                {
                    string idControl = "chk" + permiso.Modulo + permiso.Accion;
                    CheckBox chk = this.FindControl(idControl) as CheckBox;
                    if (chk != null)
                        chk.Checked = true;
                }
            }
        }

    }
}