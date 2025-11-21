using SGTO.Datos.Repositorios;
using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Negocio.DTOs.HistoriaClinica;
using SGTO.Negocio.Excepciones;
using System;
using System.Diagnostics;

namespace SGTO.Negocio.Servicios
{
    public class HistoriaClinicaService
    {
        private readonly HistoriaClinicaRepositorio _repoHistoria;
        private readonly TurnoRepositorio _repoTurno;
        private readonly TratamientoRepositorio _repoTratamiento;
        private readonly MedicoRepositorio _repoMedico;

        public HistoriaClinicaService()
        {
            _repoHistoria = new HistoriaClinicaRepositorio();
            _repoTurno = new TurnoRepositorio();
            _repoTratamiento = new TratamientoRepositorio();
            _repoMedico = new MedicoRepositorio();
        }

        public void RegistrarAtencion(HistoriaClinicaCreacionDto dto, int idUsuarioLogueado)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Diagnostico))
                throw new ExcepcionReglaNegocio("El diagnóstico es obligatorio.");

            // validar que sea un médico quien hace esta acción
            Medico medico = _repoMedico.ObtenerPorUsuarioId(idUsuarioLogueado);
            if (medico == null)
                throw new ExcepcionReglaNegocio("El usuario actual no tiene un perfil médico asociado activo.");

            // validar qu eexista el turno
            Turno turno = _repoTurno.ObtenerPorId(dto.IdTurno);
            if (turno == null)
                throw new ExcepcionReglaNegocio("El turno indicado no existe.");

            // verificar qu eel turno esté asignado al médico que atiende
            if (turno.Medico.IdMedico != medico.IdMedico)
                throw new ExcepcionReglaNegocio("No tiene permiso para atender un turno asignado a otro profesional.");

            // el turno debe estar en un estado correcto: Nuevo o reprogramdo
            if (turno.Estado != EstadoTurno.Nuevo && turno.Estado != EstadoTurno.Reprogramado)
                throw new ExcepcionReglaNegocio($"No es posible atender el turno porque se encuentra en estado '{turno.Estado}'.");

            // validar qu eel tratamiento exista
            Tratamiento tratamiento = null;
            if (dto.IdTratamiento != 0)
            {
                tratamiento = _repoTratamiento.ObtenerPorId(dto.IdTratamiento);

                if (tratamiento == null)
                    throw new ExcepcionReglaNegocio("El tratamiento no existe.");

                if (tratamiento.Especialidad.IdEspecialidad != turno.Especialidad.IdEspecialidad)
                    throw new ExcepcionReglaNegocio("Incoherencia de especialidad.");
            }
            else if (string.IsNullOrWhiteSpace(dto.TratamientoManual))
            {
                throw new ExcepcionReglaNegocio("Debe indicar un tratamiento (Selección o Manual).");
            }


            HistoriaClinicaRegistro historia = new HistoriaClinicaRegistro
            {
                TurnoOrigen = turno,
                Medico = medico,
                Especialidad = turno.Especialidad,
                TratamientoAplicado = tratamiento,
                TratamientoManual = dto.TratamientoManual,
                Diagnostico = dto.Diagnostico,
                Observaciones = dto.Observaciones,
                FechaAtencion = DateTime.Now
            };

            try
            {
                _repoHistoria.CrearTransaccional(historia);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR al generar la historia clínica: " + ex.Message);
                throw new Exception("Ocurrió un error al intentar guardar la historia clínica. Por favor intente nuevamente.");
            }
        }

    }
}
