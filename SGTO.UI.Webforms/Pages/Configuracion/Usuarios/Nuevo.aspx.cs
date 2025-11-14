using SGTO.Comun.Validacion;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.DTOs.Medicos;
using SGTO.Negocio.DTOs.Usuarios;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Servicios;
using SGTO.UI.Webforms.MasterPages;
using SGTO.UI.Webforms.Modelos.Medicos;
using SGTO.UI.Webforms.Utils;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Configuracion.Usuarios
{
    public partial class Nuevo : System.Web.UI.Page
    {

        private readonly EspecialidadService _servicioEspecialidad = new EspecialidadService();
        private readonly UsuarioService _servicioUsuario = new UsuarioService();


        // para poder mantener el estado de la ui para manejar los horarios tenemos que 
        // usar ViewState permite manejarlo sn perder los datos, por cada día debemos mantener los datos
        private Dictionary<string, List<HorarioSemanalItemUi>> HorariosDias
        {
            get
            {
                if (ViewState["HorariosDias"] == null)
                {
                    ViewState["HorariosDias"] = new Dictionary<string, List<HorarioSemanalItemUi>>()
                    {
                        { "Lunes", new List<HorarioSemanalItemUi>() },
                        { "Martes", new List<HorarioSemanalItemUi>() },
                        { "Miercoles", new List<HorarioSemanalItemUi>() },
                        { "Jueves", new List<HorarioSemanalItemUi>() },
                        { "Viernes", new List<HorarioSemanalItemUi>() },
                        { "Sabado", new List<HorarioSemanalItemUi>() },
                        { "Domingo", new List<HorarioSemanalItemUi>() }
                    };
                }
                return (Dictionary<string, List<HorarioSemanalItemUi>>)ViewState["HorariosDias"];
            }
            set => ViewState["HorariosDias"] = value;
        }


        private readonly Dictionary<string, string> _mapaDias = new Dictionary<string, string>()
        {
            { "repLunes", "Lunes" },
            { "repMartes", "Martes" },
            { "repMiercoles", "Miercoles" },
            { "repJueves",  "Jueves" },
            { "repViernes", "Viernes" },
            { "repSabado", "Sabado" },
            { "repDomingo", "Domingo" }
        };



        protected void Page_Load(object sender, EventArgs e)
        {

            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Configuracion");
                master.EstablecerTituloSeccion(this.Page.Title);
            }

            if (!IsPostBack)
            {
                CalcularFechaMaximaValida();
                CargarHorarioClinica();
                BindearTodosLosDias();
                CargarEspecialidades();

                lblPaso.InnerText = "Paso 1 de 2 · Seleccione el rol del usuario.";
            }
        }

        private void BindearTodosLosDias()
        {
            repLunes.DataSource = HorariosDias["Lunes"]; repLunes.DataBind();
            repMartes.DataSource = HorariosDias["Martes"]; repMartes.DataBind();
            repMiercoles.DataSource = HorariosDias["Miercoles"]; repMiercoles.DataBind();
            repJueves.DataSource = HorariosDias["Jueves"]; repJueves.DataBind();
            repViernes.DataSource = HorariosDias["Viernes"]; repViernes.DataBind();
            repSabado.DataSource = HorariosDias["Sabado"]; repSabado.DataBind();
            repDomingo.DataSource = HorariosDias["Domingo"]; repDomingo.DataBind();
        }

        protected void AgregarRango_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            string dia = btn.CommandArgument; // Ej: "Lunes"

            HorariosDias[dia].Add(new HorarioSemanalItemUi("", ""));

            string repeaterId = "rep" + dia;

            // como el botón está fuera del repeater pero dentro del UpdatePanel, 
            // necesitamos buscar el control.
            var repeater = (Repeater)updDisponibilidad.FindControl(repeaterId);

            if (repeater != null)
            {
                repeater.DataSource = HorariosDias[dia];
                repeater.DataBind();
            }
            else
            {
                BindearTodosLosDias();
            }
        }


        protected void Rango_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "Eliminar") return;

            var rep = (Repeater)source;
            string dia = _mapaDias[rep.ID];

            int index = Convert.ToInt32(e.CommandArgument);

            if (index >= 0 && index < HorariosDias[dia].Count)
                HorariosDias[dia].RemoveAt(index);

            rep.DataSource = HorariosDias[dia];
            rep.DataBind();
        }


        protected void Rango_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
                e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var rango = (HorarioSemanalItemUi)e.Item.DataItem;

            var ddlInicio = (DropDownList)e.Item.FindControl("ddlInicio");
            var ddlFin = (DropDownList)e.Item.FindControl("ddlFin");

            List<string> horas = (List<string>)ViewState["HorasClinica"];

            ddlInicio.Items.Add(new ListItem("Seleccione...", ""));
            ddlFin.Items.Add(new ListItem("Seleccione...", ""));

            foreach (var h in horas)
            {
                ddlInicio.Items.Add(new ListItem(h, h));
                ddlFin.Items.Add(new ListItem(h, h));
            }

            if (!string.IsNullOrEmpty(rango.Inicio))
                ddlInicio.SelectedValue = rango.Inicio;

            if (!string.IsNullOrEmpty(rango.Fin))
                ddlFin.SelectedValue = rango.Fin;

            ddlInicio.AutoPostBack = true;
            ddlFin.AutoPostBack = true;
        }


        protected void Horario_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ddl = (DropDownList)sender;
            var item = (RepeaterItem)ddl.NamingContainer;
            var rep = (Repeater)item.Parent;

            string dia = _mapaDias[rep.ID];
            int index = item.ItemIndex;

            bool esInicio = ddl.ID == "ddlInicio";
            string valor = ddl.SelectedValue;

            if (esInicio)
                HorariosDias[dia][index].Inicio = valor;
            else
                HorariosDias[dia][index].Fin = valor;
        }

        private void CargarHorarioClinica()
        {
            try
            {
                var (horaApertura, horaCierre) = _servicioUsuario.ObtenerHorarioClinica();
                lblHorarioClinica.Text = $"{horaApertura:hh\\:mm} a {horaCierre:hh\\:mm}";

                List<string> horas = new List<string>();

                for (var h = horaApertura; h <= horaCierre; h = h.Add(TimeSpan.FromHours(1)))
                    horas.Add(h.ToString(@"hh\:mm"));

                ViewState["HorasClinica"] = horas;
            }
            catch
            {
                lblHorarioClinica.Text = "No disponible";
                ViewState["HorasClinica"] = new List<string>();
            }
        }


        private List<HorarioSemanalDto> ObtenerHorariosDTO()
        {
            var listaFinal = new List<HorarioSemanalDto>();
            int diaContador = 1; // 1=Lunes, 2=Martes...

            // cada key del diccionario representa el día y su listado de horarios, por ejemplo lunes = [...]
            foreach (var kv in HorariosDias)
            {
                // creamos una lista temporal solo para los rangos del día actual
                var rangosDelDia = new List<HorarioSemanalDto>();

                foreach (var rangoUi in kv.Value)
                {
                    if (string.IsNullOrEmpty(rangoUi.Inicio) || string.IsNullOrEmpty(rangoUi.Fin))
                        throw new ArgumentException("Debe completar todos los rangos horarios para el día " + kv.Key + ".");

                    TimeSpan ini, fin;
                    if (!TimeSpan.TryParse(rangoUi.Inicio, out ini) || !TimeSpan.TryParse(rangoUi.Fin, out fin))
                        throw new ArgumentException("Formato de hora inválido para el día " + kv.Key + ".");

                    if (ini >= fin)
                        throw new ArgumentException("En el día " + kv.Key + ", la hora de inicio (" + ini.ToString(@"hh\:mm") + ") debe ser menor que la hora de fin (" + fin.ToString(@"hh\:mm") + ").");

                    // agregamos el rango a la lista temporal para el dia actual
                    rangosDelDia.Add(new HorarioSemanalDto
                    {
                        DiaSemana = (byte)diaContador,
                        HoraInicio = ini,
                        HoraFin = fin,
                        Estado = "A"
                    });
                }

                // si hay más de un rango se busca por solapamientos
                if (rangosDelDia.Count > 1)
                {
                    if (HaySolapamiento(rangosDelDia))
                    {
                        throw new ArgumentException("Existen rangos de horarios solapados para el día " + kv.Key + ". Por favor, corríjalos.");
                    }
                }
                listaFinal.AddRange(rangosDelDia);
                diaContador++;
            }
            return listaFinal;
        }


        private bool HaySolapamiento(List<HorarioSemanalDto> rangos)
        {
            rangos.Sort(CompararHorarios); // ordenar por hora de inicio

            // comparar cada rango con el siguiente rango
            for (int i = 0; i < rangos.Count - 1; i++)
            {
                HorarioSemanalDto rangoActual = rangos[i];
                HorarioSemanalDto rangoSiguiente = rangos[i + 1];

                // si la hora del fin del rango actual es mayor que
                // la hora del inicio del siguiente, entonces significa que se solapan estos.
                if (rangoActual.HoraFin > rangoSiguiente.HoraInicio)
                {
                    return true;
                }
            }
            return false;
        }



        private int CompararHorarios(HorarioSemanalDto a, HorarioSemanalDto b)
        {
            // camparamos usando los métodos de timespan que son ma´s seguros
            return a.HoraInicio.CompareTo(b.HoraInicio);
        }


        private void CargarEspecialidades()
        {
            try
            {
                cblEspecialidades.Items.Clear();
                var lista = _servicioEspecialidad.ObtenerTodasDto("activas");

                foreach (EspecialidadDto esp in lista)
                {
                    ListItem item = new ListItem(esp.Nombre, esp.IdEspecialidad.ToString());
                    cblEspecialidades.Items.Add(item);
                }
            }
            catch
            {
                cblEspecialidades.Items.Add(new ListItem("Error al cargar especialidades", ""));
            }
        }




        protected void ddlRol_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool ok = !string.IsNullOrEmpty(ddlRol.SelectedValue);

            panelCamposGenerales.Visible = ok;
            panelAcciones.Visible = ok;
            panelCamposMedico.Visible = (ddlRol.SelectedValue == "Médico");

            lblPaso.InnerText = ok
                ? "Paso 2 de 2 · Complete los datos del usuario."
                : "Paso 1 de 2 · Seleccione el rol del usuario.";
        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarCamposFormulario();

                UsuarioCrearDto usuarioDto = new UsuarioCrearDto()
                {
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    NombreUsuario = txtNombreUsuario.Text.Trim(),
                    Password = txtPassword.Text,
                    ConfirmarPassword = txtConfirmarPassword.Text,
                    IdRol = ObtenerIdRol(ddlRol.SelectedValue),
                    Estado = ddlEstado.SelectedValue
                };

                MedicoCrearDto medicoDto = null;

                if (ddlRol.SelectedValue == "Médico")
                {
                    bool tieneEspecialidad = false;
                    foreach (ListItem item in cblEspecialidades.Items)
                    {
                        if (item.Selected)
                        {
                            tieneEspecialidad = true;
                            break;
                        }
                    }
                    if (!tieneEspecialidad)
                        throw new ArgumentException("Debe seleccionar al menos una especialidad.");



                    medicoDto = new MedicoCrearDto()
                    {
                        Nombre = txtNombre.Text.Trim(),
                        Apellido = txtApellido.Text.Trim(),
                        NumeroDocumento = txtDni.Text.Trim(),
                        Genero = ddlGenero.SelectedValue,
                        FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text),
                        Telefono = txtTelefono.Text.Trim(),
                        Matricula = txtMatricula.Text.Trim(),
                        Estado = ddlEstado.SelectedValue
                    };


                    medicoDto.IdEspecialidades = ObtenerEspecialidadesSeleccionadas();
                    medicoDto.HorariosSemanales = ObtenerHorariosDTO();

                    if (medicoDto.HorariosSemanales == null || medicoDto.HorariosSemanales.Count == 0)
                        throw new ArgumentException("Debe configurar al menos un día y horario de atención para el médico.");

                }

                _servicioUsuario.Crear(usuarioDto, medicoDto);

                MensajeUiHelper.SetearYMostrar(
                    this.Page,
                    "Usuario creado",
                    "El usuario se ha creado correctamente.",
                    "Resultado",
                    VirtualPathUtility.ToAbsolute("~/Pages/Configuracion/Usuarios/Index.aspx"),
                    "abrirModalResultado"
                );
            }
            catch (ArgumentException ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page,
                    "Dato inválido",
                    ex.Message,
                    "Resultado",
                    null,
                    "abrirModalResultado");
            }
            catch (ExcepcionReglaNegocio ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page,
                    "Operación no permitida",
                    ex.Message,
                    "Resultado",
                    null,
                    "abrirModalResultado");
            }
            catch (Exception ex)
            {
                MensajeUiHelper.SetearYMostrar(this.Page,
                    "Error inesperado",
                    "Ocurrió un error al registrar el usuario. " + ex.Message,
                    "Resultado",
                    null,
                    "abrirModalResultado");
            }
        }


        private void ValidarCamposFormulario()
        {
            if (!ValidadorCampos.EsTextoObligatorio(txtNombre.Text))
                throw new ArgumentException("El nombre es obligatorio.");

            if (!ValidadorCampos.EsSoloLetrasYEspacios(txtNombre.Text))
                throw new ArgumentException("El nombre solo puede contener letras y espacios.");

            if (!ValidadorCampos.EsSoloLetrasYEspacios(txtApellido.Text))
                throw new ArgumentException("El apellido solo puede contener letras y espacios.");

            if (!ValidadorCampos.EsEmailValido(txtEmail.Text))
                throw new ArgumentException("El email no tiene un formato válido.");

            if (string.IsNullOrWhiteSpace(txtNombreUsuario.Text))
                throw new ArgumentException("El nombre de usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(txtPassword.Text) || string.IsNullOrWhiteSpace(txtConfirmarPassword.Text))
                throw new ArgumentException("Debe ingresar y confirmar la contraseña.");

            if (txtPassword.Text != txtConfirmarPassword.Text)
                throw new ArgumentException("Las contraseñas no coinciden.");

            if (ddlRol.SelectedValue == "Médico")
            {
                if (!ValidadorCampos.EsEnteroPositivo(txtDni.Text))
                    throw new ArgumentException("El DNI debe ser numérico.");

                if (!DateTime.TryParse(txtFechaNacimiento.Text, out DateTime fechaNac))
                    throw new ArgumentException("Debe ingresar una fecha de nacimiento válida.");

                if (!ValidadorCampos.EsTelefonoValido(txtTelefono.Text))
                    throw new ArgumentException("El teléfono no tiene un formato válido.");

                if (string.IsNullOrWhiteSpace(txtMatricula.Text))
                    throw new ArgumentException("La matrícula es obligatoria para el médico.");
            }
        }



        private List<int> ObtenerEspecialidadesSeleccionadas()
        {
            var lista = new List<int>();
            foreach (ListItem item in cblEspecialidades.Items)
                if (item.Selected)
                    lista.Add(int.Parse(item.Value));
            return lista;
        }

        private int ObtenerIdRol(string rol)
        {
            switch (rol)
            {
                case "Administrador": return 1;
                case "Recepcionista": return 2;
                case "Médico": return 3;
                default: throw new ExcepcionReglaNegocio("Rol desconocido.");
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Configuracion/Usuarios/Index.aspx");
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Configuracion/Usuarios/Index.aspx");
        }

        private void CalcularFechaMaximaValida()
        {
            rvFechaNac.MaximumValue = DateTime.Now.AddYears(-20).ToString("yyyy-MM-dd");
        }

    }
}