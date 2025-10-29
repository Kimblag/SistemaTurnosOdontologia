using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Dominio.ObjetosValor;
using SGTO.UI.Webforms.MasterPages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SGTO.UI.Webforms.Pages.Pacientes
{
    public partial class Detalle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master is SiteMaster master)
            {
                master.EstablecerOpcionMenuActiva("Pacientes");
                master.EstablecerTituloSeccion(this.Page.Title);
            }
            if (!IsPostBack)
                CargarDetallePaciente();
        }

        protected void gvTurnosPaciente_PageIndexChanging(object sender, GridViewPageEventArgs e) { }
        protected void gvTurnosPaciente_RowCommand(object sender, GridViewCommandEventArgs e) { }

        private void CargarDetallePaciente()
        {
            // método de test para probar vista detalle
            // datos paciente test
            var cobertura = new Cobertura("OSDE", "Cobertura médica prepaga");
            var plan = new Plan("310", 0.90m, cobertura, "Plan 310 odontológico");

            var paciente = new Paciente(
                idPaciente: 1,
                nombre: "Ana María",
                apellido: "García",
                dni: new DocumentoIdentidad("28123456"),
                fechaNacimiento: new DateTime(1980, 5, 15),
                genero: Genero.Femenino,
                telefono: new Telefono("11-5555-4444"),
                email: new Email("anamgarcia@email.com"),
                cobertura: cobertura,
                plan: plan,
                turnos: new List<Turno>(),
                historiaClinica: new List<HistoriaClinicaRegistro>()
            );

            // medicos y especialidades de test
            var especialidadGeneral = new Especialidad("Odontología General", "Tratamientos dentales generales");
            var tratamientos = new List<Tratamiento>
            {
                new Tratamiento("Consulta odontológica", "Evaluación inicial del paciente", 8000, especialidadGeneral),
                new Tratamiento("Limpieza dental", "Limpieza profunda y profilaxis", 9500, especialidadGeneral),
                new Tratamiento("Control", "Control post tratamiento", 7000, especialidadGeneral)
            };
            especialidadGeneral.TratamientosAsociados.AddRange(tratamientos);

            var rolMedico = new Rol("Médico", "Acceso a gestión de pacientes y turnos", new List<Permiso>());
            var usuarioDrPerez = new Usuario(
                "Juan", "Pérez", new Email("juan.perez@clinic.com"),
                "jperez", "hashed1234", rolMedico
            );

            var medico = new Medico(
                idMedico: 1,
                nombre: "Juan",
                apellido: "Pérez",
                dni: new DocumentoIdentidad("27333444"),
                fechaNacimiento: new DateTime(1975, 3, 12),
                genero: Genero.Masculino,
                telefono: new Telefono("11-6000-0001"),
                email: new Email("juan.perez@clinic.com"),
                matricula: "MP-45892",
                especialidades: new List<Especialidad> { especialidadGeneral },
                turnosAsignados: new List<Turno>(),
                usuario: usuarioDrPerez
            );

            // turnos test
            var turnos = new List<Turno>
            {
                new Turno(
                    paciente,
                    medico,
                    especialidadGeneral,
                    tratamientos[0],
                    new HorarioTurno(
                        new DateTime(2023, 7, 20, 10, 0, 0),
                        new DateTime(2023, 7, 20, 11, 0, 0)
                    )
                )
                {
                    Estado = EstadoTurno.Cerrado,
                    Observaciones = "Control general post limpieza"
                },

                new Turno(
                    paciente,
                    medico,
                    especialidadGeneral,
                    tratamientos[1],
                    new HorarioTurno(
                        new DateTime(2023, 5, 15, 15, 30, 0),
                        new DateTime(2023, 5, 15, 16, 30, 0)
                    )
                )
                {
                    Estado = EstadoTurno.Cerrado,
                    Observaciones = "Limpieza anual"
                },

                new Turno(
                    paciente,
                    medico,
                    especialidadGeneral,
                    tratamientos[2],
                    new HorarioTurno(
                        new DateTime(2023, 3, 2, 9, 0, 0),
                        new DateTime(2023, 3, 2, 10, 0, 0)
                    )
                )
                {
                    Estado = EstadoTurno.Cerrado,
                    Observaciones = "Consulta inicial del paciente"
                }
            };

            paciente.Turnos = turnos;


            var table = new DataTable();
            table.Columns.Add("Fecha");
            table.Columns.Add("Hora");
            table.Columns.Add("Profesional");
            table.Columns.Add("Motivo");
            table.Columns.Add("EstadoTexto");
            foreach (var t in paciente.Turnos)
            {
                table.Rows.Add(
                    t.Horario.Inicio.ToString("dd/MM/yyyy"),
                    t.Horario.Inicio.ToString("HH:mm"),
                    $"{t.Medico.Nombre} {t.Medico.Apellido}",
                    t.Tratamiento?.Nombre ?? "—",
                    t.Estado.ToString()
                );
            }
            gvTurnosPaciente.DataSource = table;
            gvTurnosPaciente.DataBind();


            lblNombreCompleto.Text = $"{paciente.Nombre} {paciente.Apellido}";
            lblDni.Text = paciente.Dni.Numero;
            lblFechaNacimiento.Text = paciente.FechaNacimiento.ToString("dd/MM/yyyy");
            lblGenero.Text = paciente.Genero.ToString();
            lblTelefono.Text = paciente.Telefono.Numero;
            lblEmail.Text = paciente.Email.Valor;
            lblCobertura.Text = paciente.Cobertura.Nombre;
            lblPlan.Text = paciente.Plan.Nombre;
            lblEstado.Text = paciente.Estado.ToString();
        }

    }
}