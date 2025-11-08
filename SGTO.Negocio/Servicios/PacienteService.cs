using SGTO.Comun.Validacion;
using SGTO.Datos.Repositorios;
using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Dominio.ObjetosValor;
using SGTO.Negocio.DTOs.Pacientes;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Mappers;
using System;
using System.Collections.Generic;

namespace SGTO.Negocio.Servicios
{
    public class PacienteService
    {
        private readonly PacienteRepositorio _repositorioPaciente;
        private readonly PlanRepositorio _repositorioPlan;
        private readonly CoberturaRepositorio _repositorioCobertura;

        public PacienteService()
        {
            _repositorioPaciente = new PacienteRepositorio();
            _repositorioCobertura = new CoberturaRepositorio();
            _repositorioPlan = new PlanRepositorio();
        }


        public List<PacienteListadoDto> Listar(string estado = null)
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

        public bool ExisteNumeroDocumento(string numeroDocumento)
        {
            try
            {
                return _repositorioPaciente.ExistePorDni(numeroDocumento);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void Crear(PacienteCreacionDto pacienteDto)
        {
            if (pacienteDto == null)
                throw new ArgumentNullException(nameof(pacienteDto));

            // validar que no exista un paciente con el mismo dni
            if (_repositorioPaciente.ExistePorDni(pacienteDto.Dni))
                throw new ExcepcionReglaNegocio("El DNI ingresado ya se encuentra registrado.");

            // aunque solo se listan coberturas y planes activos, se valida que todo esté bien.

            // validar que la cobertura seleccionada esté activa
            Cobertura cobertura = _repositorioCobertura.ObtenerPorId(pacienteDto.IdCobertura);
            if (cobertura == null || cobertura.Estado != EstadoEntidad.Activo)
                throw new ExcepcionReglaNegocio("La cobertura seleccionada no es válida o está inactiva.");

            // validar si la cobertura seleccionada tiene planes asociados: si tiene planes debe haber seleccionado alguno
            List<Plan> planesActivos = _repositorioPlan.ListarPorCobertura(cobertura.IdCobertura, EstadoEntidad.Activo.ToString());

            if (planesActivos != null && planesActivos.Count > 0 && pacienteDto.IdPlan == 0)
                throw new ExcepcionReglaNegocio("La cobertura seleccionada tiene planes disponibles, debe seleccionar uno.");


            // si hay un plan seleccionado validar qu eesté activo
            Plan plan = null;
            if (pacienteDto.IdPlan != 0)
            {
                plan = _repositorioPlan.ObtenerPorId(pacienteDto.IdPlan);

                if (plan == null || plan.Estado != EstadoEntidad.Activo)
                    throw new ExcepcionReglaNegocio("El plan seleccionado no es válido o está inactivo.");

                // validar si el plan pertenece a la cobertura seleccionada.
                if (plan.Cobertura.IdCobertura != cobertura.IdCobertura)
                    throw new ExcepcionReglaNegocio("El plan no pertenece a la cobertura seleccionada.");
            }

            Paciente nuevoPaciente = new Paciente()
            {
                Nombre = ValidadorCampos.CapitalizarTexto(pacienteDto.Nombre),
                Apellido = ValidadorCampos.CapitalizarTexto(pacienteDto.Apellido),
                Dni = new DocumentoIdentidad(pacienteDto.Dni),
                FechaNacimiento = pacienteDto.FechaNacimiento,
                Genero = EnumeracionMapperNegocio.MapearGenero(pacienteDto.Genero),
                Telefono = new Telefono(pacienteDto.Telefono),
                Email = new Email(pacienteDto.Email),
                Cobertura = cobertura,
                Plan = plan,
                Estado = EstadoEntidad.Activo,
                FechaAlta = DateTime.Now,
                FechaModificacion = DateTime.Now
            };

            try
            {
                _repositorioPaciente.Crear(nuevoPaciente);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void Modificar()
        {

        }



    }
}
