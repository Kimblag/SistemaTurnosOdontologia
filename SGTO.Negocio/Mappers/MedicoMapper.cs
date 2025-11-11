using SGTO.Dominio.Entidades;
using SGTO.Dominio.ObjetosValor;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.DTOs.Medicos;
using System;
using System.Collections.Generic;

namespace SGTO.Negocio.Mappers
{
    public static class MedicoMapper
    {
        public static Medico MapearDesdeCrearDto(MedicoCrearDto dto, int idUsuario)
        {
            var especialidades = new List<Especialidad>();
            if (dto.IdEspecialidad > 0)
                especialidades.Add(new Especialidad { IdEspecialidad = dto.IdEspecialidad });

            return new Medico
            {
                Nombre = dto.Nombre.Trim(),
                Apellido = dto.Apellido.Trim(),
                Dni = new DocumentoIdentidad(dto.NumeroDocumento.Trim()),
                FechaNacimiento = dto.FechaNacimiento,
                Genero = EnumeracionMapperNegocio.MapearGenero(dto.Genero),
                Telefono = new Telefono(dto.Telefono.Trim()),
                Email = new Email(dto.Email.Trim()),
                Matricula = dto.Matricula.Trim(),
                Especialidades = especialidades,
                Usuario = new Usuario { IdUsuario = idUsuario },
                Estado = EnumeracionMapperNegocio.MapearEstadoEntidad(dto.Estado),
                FechaAlta = DateTime.Now,
                FechaModificacion = DateTime.Now
            };
        }

        public static Medico MapearDesdeEdicionDto(MedicoEdicionDto dto)
        {
            var especialidades = new List<Especialidad>();
            if (dto.IdEspecialidad > 0)
                especialidades.Add(new Especialidad { IdEspecialidad = dto.IdEspecialidad });

            return new Medico
            {
                IdMedico = dto.IdMedico,
                Nombre = dto.Nombre.Trim(),
                Apellido = dto.Apellido.Trim(),
                Dni = new DocumentoIdentidad(dto.NumeroDocumento.Trim()),
                FechaNacimiento = dto.FechaNacimiento,
                Genero = EnumeracionMapperNegocio.MapearGenero(dto.Genero),
                Telefono = new Telefono(dto.Telefono.Trim()),
                Email = new Email(dto.Email.Trim()),
                Matricula = dto.Matricula.Trim(),
                Especialidades = especialidades,
                Usuario = new Usuario { IdUsuario = dto.IdUsuario },
                Estado = EnumeracionMapperNegocio.MapearEstadoEntidad(dto.Estado),
                FechaModificacion = DateTime.Now
            };
        }


        public static MedicoDetalleDto MapearADetalleDto(Medico medico)
        {
            return new MedicoDetalleDto
            {
                IdMedico = medico.IdMedico,
                NumeroDocumento = medico.Dni.Numero,
                FechaNacimiento = medico.FechaNacimiento,
                Genero = medico.Genero.ToString(),
                Telefono = medico.Telefono.Numero,
                Email = medico.Email.Valor,
                Matricula = medico.Matricula,
                IdEspecialidad = medico.Especialidades != null && medico.Especialidades.Count > 0
                    ? medico.Especialidades[0].IdEspecialidad
                    : 0,
                Especialidad = medico.Especialidades != null && medico.Especialidades.Count > 0
                    ? medico.Especialidades[0].Nombre
                    : null,
                Estado = medico.Estado.ToString()
            };
        }

        public static MedicoEdicionDto MapearAEdicionDto(Medico medico)
        {
            return new MedicoEdicionDto
            {
                IdUsuario = medico.Usuario.IdUsuario,
                IdMedico = medico.IdMedico,
                NumeroDocumento = medico.Dni.Numero,
                FechaNacimiento = medico.FechaNacimiento,
                Genero = medico.Genero.ToString(),
                Telefono = medico.Telefono.Numero,
                Email = medico.Email.Valor,
                Matricula = medico.Matricula,
                IdEspecialidad = medico.Especialidades != null && medico.Especialidades.Count > 0
                    ? medico.Especialidades[0].IdEspecialidad
                    : 0,
                Estado = medico.Estado.ToString()
            };
        }

    }
}
