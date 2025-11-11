using SGTO.Dominio.Entidades;
using SGTO.Dominio.ObjetosValor;
using SGTO.Negocio.DTOs;
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
    }
}
