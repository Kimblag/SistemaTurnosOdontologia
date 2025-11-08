using SGTO.Datos.Mappers;
using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs.Pacientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGTO.Negocio.Mappers
{
    public static class PacienteMapper
    {
        public static PacienteListadoDto MapearADto(Paciente entidad)
        {
            if (entidad == null)
                return null;

            string nombreCompleto = string.Empty;

            if (!string.IsNullOrWhiteSpace(entidad.Apellido) || !string.IsNullOrWhiteSpace(entidad.Nombre))
            {
                nombreCompleto = $"{entidad.Apellido}, {entidad.Nombre}".Trim(',', ' ');
            }

            var dto = new PacienteListadoDto
            {
                IdPaciente = entidad.IdPaciente,
                NombreCompleto = nombreCompleto,
                Dni = entidad.Dni != null ? entidad.Dni.Numero : string.Empty,
                Telefono = entidad.Telefono != null ? entidad.Telefono.Numero : string.Empty,
                Email = entidad.Email != null ? entidad.Email.Valor : string.Empty,
                Estado = entidad.Estado.ToString(),

                IdCobertura = entidad.Cobertura != null ? (int?)entidad.Cobertura.IdCobertura : null,
                NombreCobertura = entidad.Cobertura != null ? entidad.Cobertura.Nombre : string.Empty,

                IdPlan = entidad.Plan != null ? (int?)entidad.Plan.IdPlan : null,
                NombrePlan = entidad.Plan != null ? entidad.Plan.Nombre : string.Empty
            };

            return dto;
        }
        public static PacienteEdicionDto MapearAEdicionDto(Paciente entidad)
        {
            if (entidad == null)
                return null;

            var dto = new PacienteEdicionDto
            {
                IdPaciente = entidad.IdPaciente,
                Nombre = entidad.Nombre,
                Apellido = entidad.Apellido,
                Dni = entidad.Dni != null ? entidad.Dni.Numero : string.Empty,
                FechaNacimiento = entidad.FechaNacimiento,
                Genero = EnumeracionMapper.ObtenerChar(entidad.Genero),
                Telefono = entidad.Telefono != null ? entidad.Telefono.Numero : string.Empty,
                Email = entidad.Email != null ? entidad.Email.Valor : string.Empty,
                IdCobertura = entidad.Cobertura != null ? (int?)entidad.Cobertura.IdCobertura : null,
                IdPlan = entidad.Plan != null ? (int?)entidad.Plan.IdPlan : null,
                Estado = EnumeracionMapper.ObtenerChar(entidad.Estado)
            };

            return dto;
        }


        public static List<PacienteListadoDto> MapearAListado(List<Paciente> pacientes)
        {
            List<PacienteListadoDto> pacientesDtos = new List<PacienteListadoDto>();
            foreach (Paciente paciente in pacientes)
            {
                pacientesDtos.Add(MapearADto(paciente));
            }
            return pacientesDtos;
        }



    }
}
