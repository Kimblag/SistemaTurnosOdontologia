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

        private const string ROL_ADMIN = "Administrador";
        private const string ROL_MEDICO = "Médico";
        private const string ROL_RECEPCIONISTA = "Recepcionista";

        public bool ModoEdicion { get; set; } = false;
        public bool ModoLectura { get; set; } = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            int idRol = ValidarModoEdicion();

            if (!IsPostBack)
            {
                if (idRol != 0)
                {
                    CargarDetalleRol(idRol);
                }
                else if (!ModoLectura)
                {
                    ddlEstado.SelectedValue = "Activo";
                    ddlEstado.Enabled = false;
                    // iniciar la matriz de permisos vacío
                    List<ModuloPermisosDto> matriz = _servicioRol.ObtenerMatrizPermisos(null);

                    // marcamos ver por defecto para el modo crear
                    foreach (var m in matriz) { if (m.IdPermisoVer > 0) m.AsignadoVer = true; }

                    rptPermisos.DataSource = matriz;
                    rptPermisos.DataBind();
                }

                if (ModoLectura)
                {
                    BloquearTodosLosControles();
                    btnGuardar.Visible = false;
                    btnCancelar.Visible = false;
                }

                ModalHelper.MostrarModalDesdeSession(
                    this.Page,
                    "RolMensajeTitulo",
                    "RolMensajeDesc",
                    "/Pages/Configuracion/Roles/Index"
                );
            }
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

            List<ModuloPermisosDto> matriz = _servicioRol.ObtenerMatrizPermisos(rol.IdPermisos);
            rptPermisos.DataSource = matriz;
            rptPermisos.DataBind();

            if (!ModoLectura)
            {
                AplicarReglasDeEdicionSegunRol(rol.Nombre);
            }
        }


        private void AplicarReglasDeEdicionSegunRol(string nombreRol)
        {
            string nombre = nombreRol.Trim();

            bool esAdmin = nombre.Equals(ROL_ADMIN, StringComparison.OrdinalIgnoreCase);
            bool esMedico = nombre.Equals(ROL_MEDICO, StringComparison.OrdinalIgnoreCase);
            bool esRecep = nombre.Equals(ROL_RECEPCIONISTA, StringComparison.OrdinalIgnoreCase);

            if (esAdmin)
            {
                BloquearTodosLosControles();
                btnGuardar.Visible = false;

                MostrarModalError("Información", "El rol Administrador es fundamental y no puede ser modificado.");
            }
            else if (esMedico || esRecep)
            {
                txtNombre.Enabled = false;
                txtNombre.ToolTip = "Rol de sistema: No editable.";

                txtDescripcion.Enabled = false;

                ddlEstado.Enabled = false;
                ddlEstado.ToolTip = "Rol de sistema: No se puede desactivar.";
            }
            else
            {
                txtNombre.Enabled = true;
                txtDescripcion.Enabled = true;
                ddlEstado.Enabled = true;
            }
        }

        private void BloquearTodosLosControles()
        {
            txtNombre.Enabled = false;
            txtDescripcion.Enabled = false;
            ddlEstado.Enabled = false;

            // iterar por los items creados en el repeater para poder desactivarlos en modo lectura
            foreach (RepeaterItem item in rptPermisos.Items)
            {
                // verificar que sea el item de datos
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    // lista de los permisos
                    string[] sufijos = { "Ver", "Crear", "Editar", "Activar", "Desactivar", "Eliminar" };

                    foreach (string sufijo in sufijos)
                    {
                        // buscar el control actual, usando el ID del template
                        CheckBox chk = item.FindControl("chk" + sufijo) as CheckBox;

                        if (chk != null)
                        {
                            chk.Enabled = false;
                        }
                    }
                }

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
                if (ModoEdicion) ModificarRol();
                else CrearNuevoRol();
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
            List<int> idsSeleccionados = ObtenerIdsPermisosSeleccionados();

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
            List<int> idsSeleccionados = ObtenerIdsPermisosSeleccionados();

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

        private List<int> ObtenerIdsPermisosSeleccionados()
        {
            List<int> idsSeleccionados = new List<int>();

            foreach (RepeaterItem item in rptPermisos.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    string[] sufijos = { "Ver", "Crear", "Editar", "Activar", "Desactivar", "Eliminar" };

                    foreach (string sufijo in sufijos)
                    {
                        CheckBox chk = (CheckBox)item.FindControl("chk" + sufijo);
                        HiddenField hdn = (HiddenField)item.FindControl("hdn" + sufijo);

                        if (chk != null && chk.Checked && hdn != null)
                        {
                            int idPermiso;
                            if (int.TryParse(hdn.Value, out idPermiso) && idPermiso > 0)
                            {
                                idsSeleccionados.Add(idPermiso);
                            }
                        }
                    }
                }
            }
            return idsSeleccionados;
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



    }
}