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
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Configuracion.Usuarios
{
    public partial class Editar : System.Web.UI.Page
    {

        private readonly UsuarioService _servicioUsuario = new UsuarioService();
        private readonly EspecialidadService _servicioEspecialidad = new EspecialidadService();
        private readonly HorarioSemanalService _servicioHorarioSemanal = new HorarioSemanalService();


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


        private int IdUsuario
        {
            get
            {
                int id;
                if (int.TryParse(Request.QueryString["id-usuario"], out id))
                {
                    return id;
                }
                else
                {
                    return 0;
                }
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Configuracion");
                master.EstablecerTituloSeccion(this.Page.Title);
            }

            if (!IsPostBack)
            {
                if (IdUsuario == 0)
                {
                    MensajeUiHelper.SetearYMostrar(this.Page,
                        "Usuario no encontrado",
                        "No se pudo identificar el usuario a editar.",
                        "Resultado",
                        VirtualPathUtility.ToAbsolute("~/Pages/Configuracion/Usuarios/Index.aspx"),
                        "abrirModalResultado");
                    return;
                }

                CargarHorarioClinica();
                CargarEspecialidades();
                BindearTodosLosDias();
                CargarUsuario();
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

        private void CargarUsuario()
        {
            try
            {
                UsuarioDetalleDto dto = _servicioUsuario.ObtenerDetalle(IdUsuario);
                if (dto == null)
                    throw new ExcepcionReglaNegocio("No se encontró el usuario solicitado.");

                txtNombre.Text = dto.Nombre;
                txtApellido.Text = dto.Apellido;
                txtEmail.Text = dto.Email;
                txtNombreUsuario.Text = dto.NombreUsuario;
                ddlRol.SelectedValue = dto.IdRol.ToString();
                ddlEstado.SelectedValue = dto.Estado == "Activo" ? "A" : "I";

                if (dto.Medico != null)
                {
                    panelCamposMedico.Visible = true;

                    MedicoDetalleDto medico = dto.Medico;

                    txtDni.Text = medico.NumeroDocumento;
                    ddlGenero.SelectedValue = medico.Genero;
                    txtFechaNacimiento.Text = medico.FechaNacimiento.ToString("yyyy-MM-dd");
                    txtTelefono.Text = medico.Telefono;
                    txtMatricula.Text = medico.Matricula;

                    foreach (ListItem item in cblEspecialidades.Items)
                    {
                        foreach (int idEsp in medico.IdEspecialidades)
                        {
                            if (item.Value == idEsp.ToString())
                            {
                                item.Selected = true;
                                break;
                            }
                        }
                    }

                    CargarHorariosMedicoEnRepeater(medico.IdMedico);

                    BindearTodosLosDias();
                }
                else
                {
                    panelCamposMedico.Visible = false;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                MensajeUiHelper.SetearYMostrar(this.Page,
                    "Error al cargar",
                    "No se pudieron cargar los datos del usuario.",
                    "Resultado",
                    VirtualPathUtility.ToAbsolute("~/Pages/Configuracion/Usuarios/Index.aspx"),
                    "abrirModalResultado");
            }
        }

        private void CargarHorariosMedicoEnRepeater(int idMedico)
        {
            List<HorarioSemanalDto> horarios = _servicioHorarioSemanal.ObtenerPorMedico(idMedico);

            foreach (HorarioSemanalDto h in horarios)
            {
                string dia = ObtenerNombreDia(h.DiaSemana);

                HorariosDias[dia].Add(new HorarioSemanalItemUi(
                    h.HoraInicio.ToString(@"hh\:mm"),
                    h.HoraFin.ToString(@"hh\:mm")
                ));
            }
        }

        private string ObtenerNombreDia(int dia)
        {
            switch (dia)
            {
                case 1: return "Lunes";
                case 2: return "Martes";
                case 3: return "Miercoles";
                case 4: return "Jueves";
                case 5: return "Viernes";
                case 6: return "Sabado";
                case 7: return "Domingo";
            }
            return "Lunes";
        }

        private void CargarEspecialidades()
        {
            cblEspecialidades.Items.Clear();

            try
            {
                List<EspecialidadDto> especialidades =
                    _servicioEspecialidad.ObtenerTodasDto("activas");

                foreach (EspecialidadDto esp in especialidades)
                    cblEspecialidades.Items.Add(new ListItem(esp.Nombre, esp.IdEspecialidad.ToString()));
            }
            catch
            {
                cblEspecialidades.Items.Add(new ListItem("Error al cargar especialidades", ""));
            }
        }


        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarCamposFormulario();

                UsuarioEdicionDto usuarioDto = new UsuarioEdicionDto
                {
                    IdUsuario = IdUsuario,
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    NombreUsuario = txtNombreUsuario.Text.Trim(),
                    Password = txtPassword.Text,
                    IdRol = int.Parse(ddlRol.SelectedValue),
                    Estado = ddlEstado.SelectedValue
                };

                MedicoEdicionDto medicoDto = null;

                if (ddlRol.SelectedValue == "3")
                {
                    medicoDto = new MedicoEdicionDto
                    {
                        IdUsuario = IdUsuario,
                        Nombre = txtNombre.Text.Trim(),
                        Apellido = txtApellido.Text.Trim(),
                        NumeroDocumento = txtDni.Text.Trim(),
                        Genero = ddlGenero.SelectedValue,
                        FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text),
                        Telefono = txtTelefono.Text.Trim(),
                        Matricula = txtMatricula.Text.Trim(),
                        IdEspecialidades = ObtenerEspecialidadesSeleccionadas(),
                        Estado = ddlEstado.SelectedValue,
                        HorariosSemanales = ObtenerHorariosDTO()
                    };
                }

                _servicioUsuario.Editar(usuarioDto, medicoDto);

                MensajeUiHelper.SetearYMostrar(this.Page,
                    "Cambios guardados",
                    "El usuario se actualizó correctamente.",
                    "Resultado",
                    VirtualPathUtility.ToAbsolute("~/Pages/Configuracion/Usuarios/Index.aspx"),
                    "abrirModalResultado");

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
                    "Ocurrió un error al actualizar el usuario. " + ex.Message,
                    "Resultado",
                    null,
                    "abrirModalResultado");
            }
        }

        private List<int> ObtenerEspecialidadesSeleccionadas()
        {
            List<int> lista = new List<int>();

            foreach (ListItem item in cblEspecialidades.Items)
                if (item.Selected)
                    lista.Add(int.Parse(item.Value));

            return lista;
        }


        private void CargarHorarioClinica()
        {
            try
            {
                var (horaApertura, horaCierre) = _servicioUsuario.ObtenerHorarioClinica();
                lblHorarioClinica.Text = $"{horaApertura:hh\\:mm} a {horaCierre:hh\\:mm}";

                List<string> horas = new List<string>();

                for (TimeSpan h = horaApertura; h <= horaCierre; h = h.Add(TimeSpan.FromHours(1)))
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
            int diaContador = 1;

            foreach (var kv in HorariosDias)
            {
                var rangosDelDia = new List<HorarioSemanalDto>();

                foreach (var rangoUi in kv.Value)
                {
                    if (string.IsNullOrEmpty(rangoUi.Inicio) ||
                        string.IsNullOrEmpty(rangoUi.Fin))
                        throw new ArgumentException("Debe completar todos los rangos horarios para el día " + kv.Key + ".");

                    TimeSpan ini, fin;

                    if (!TimeSpan.TryParse(rangoUi.Inicio, out ini) ||
                        !TimeSpan.TryParse(rangoUi.Fin, out fin))
                        throw new ArgumentException("Formato de hora inválido para el día " + kv.Key + ".");

                    if (ini >= fin)
                        throw new ArgumentException("En el día "
                            + kv.Key + ", la hora de inicio ("
                            + ini.ToString(@"hh\:mm")
                            + ") debe ser menor que la hora de fin ("
                            + fin.ToString(@"hh\:mm") + ").");

                    rangosDelDia.Add(new HorarioSemanalDto
                    {
                        DiaSemana = (byte)diaContador,
                        HoraInicio = ini,
                        HoraFin = fin,
                        Estado = "A"
                    });
                }

                if (rangosDelDia.Count > 1)
                {
                    if (HaySolapamiento(rangosDelDia))
                        throw new ArgumentException("Existen rangos de horarios solapados para el día " + kv.Key + ".");
                }

                listaFinal.AddRange(rangosDelDia);
                diaContador++;
            }

            return listaFinal;
        }

        private bool HaySolapamiento(List<HorarioSemanalDto> rangos)
        {
            rangos.Sort(CompararHorarios);

            for (int i = 0; i < rangos.Count - 1; i++)
                if (rangos[i].HoraFin > rangos[i + 1].HoraInicio)
                    return true;

            return false;
        }

        private int CompararHorarios(HorarioSemanalDto a, HorarioSemanalDto b)
        {
            return a.HoraInicio.CompareTo(b.HoraInicio);
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

            if (ddlRol.SelectedValue == "3")
            {
                if (!ValidadorCampos.EsEnteroPositivo(txtDni.Text))
                    throw new ArgumentException("El DNI debe ser numérico.");

                if (!DateTime.TryParse(txtFechaNacimiento.Text, out DateTime fechaNac))
                    throw new ArgumentException("Debe ingresar una fecha de nacimiento válida.");

                if (fechaNac > DateTime.Now.AddYears(-20))
                    throw new ArgumentException("El médico debe tener al menos 20 años.");

                if (!ValidadorCampos.EsTelefonoValido(txtTelefono.Text))
                    throw new ArgumentException("El teléfono no tiene un formato válido.");

                if (string.IsNullOrWhiteSpace(txtMatricula.Text))
                    throw new ArgumentException("La matrícula es obligatoria.");
            }
        }


        protected void AgregarRango_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string dia = btn.CommandArgument; // "Lunes", "Martes", etc.

            if (!HorariosDias.ContainsKey(dia))
                return;

            HorariosDias[dia].Add(new HorarioSemanalItemUi("", ""));

            string repeaterId = "rep" + dia;

            Repeater repeater = (Repeater)updDisponibilidad.FindControl(repeaterId);

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
            if (e.CommandName != "Eliminar")
                return;

            Repeater rep = (Repeater)source;

            if (!_mapaDias.ContainsKey(rep.ID))
                return;

            string dia = _mapaDias[rep.ID];

            int index;
            if (!int.TryParse(e.CommandArgument.ToString(), out index))
                return;

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

            HorarioSemanalItemUi rango = (HorarioSemanalItemUi)e.Item.DataItem;

            DropDownList ddlInicio = (DropDownList)e.Item.FindControl("ddlInicio");
            DropDownList ddlFin = (DropDownList)e.Item.FindControl("ddlFin");

            List<string> horas = ViewState["HorasClinica"] as List<string>;
            if (horas == null)
                horas = new List<string>();

            ddlInicio.Items.Clear();
            ddlFin.Items.Clear();

            ddlInicio.Items.Add(new ListItem("Seleccione...", ""));
            ddlFin.Items.Add(new ListItem("Seleccione...", ""));

            foreach (string h in horas)
            {
                ddlInicio.Items.Add(new ListItem(h, h));
                ddlFin.Items.Add(new ListItem(h, h));
            }

            if (!string.IsNullOrEmpty(rango.Inicio) &&
                ddlInicio.Items.FindByValue(rango.Inicio) != null)
            {
                ddlInicio.SelectedValue = rango.Inicio;
            }

            if (!string.IsNullOrEmpty(rango.Fin) &&
                ddlFin.Items.FindByValue(rango.Fin) != null)
            {
                ddlFin.SelectedValue = rango.Fin;
            }

            ddlInicio.AutoPostBack = true;
            ddlFin.AutoPostBack = true;
        }


        protected void Horario_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            RepeaterItem item = (RepeaterItem)ddl.NamingContainer;
            Repeater rep = (Repeater)item.Parent;

            if (!_mapaDias.ContainsKey(rep.ID))
                return;

            string dia = _mapaDias[rep.ID];
            int index = item.ItemIndex;

            bool esInicio = ddl.ID == "ddlInicio";
            string valor = ddl.SelectedValue;

            if (!HorariosDias.ContainsKey(dia))
                return;

            if (index < 0 || index >= HorariosDias[dia].Count)
                return;

            if (esInicio)
                HorariosDias[dia][index].Inicio = valor;
            else
                HorariosDias[dia][index].Fin = valor;
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Configuracion/Usuarios/Index.aspx", false);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Configuracion/Usuarios/Index.aspx", false);
        }


    }
}