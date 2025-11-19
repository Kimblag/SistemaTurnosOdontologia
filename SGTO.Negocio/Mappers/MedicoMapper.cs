using SGTO.Comun.DTOs;
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
            List<Especialidad> especialidades = new List<Especialidad>();
            if (dto.IdEspecialidades.Count > 0)
            {
                foreach (int idEspecialidad in dto.IdEspecialidades)
                {
                    especialidades.Add(new Especialidad { IdEspecialidad = idEspecialidad });

                }
            }

            return new Medico
            {
                Nombre = dto.Nombre.Trim(),
                Apellido = dto.Apellido.Trim(),
                Dni = new DocumentoIdentidad(dto.NumeroDocumento.Trim()),
                FechaNacimiento = dto.FechaNacimiento,
                Genero = EnumeracionMapperNegocio.MapearGenero(dto.Genero),
                Telefono = new Telefono(dto.Telefono.Trim()),
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
            List<Especialidad> especialidades = new List<Especialidad>();
            if (dto.IdEspecialidades.Count > 0)
            {
                foreach (int idEspecialidad in dto.IdEspecialidades)
                {
                    especialidades.Add(new Especialidad { IdEspecialidad = idEspecialidad });
                }
            }

            return new Medico
            {
                IdMedico = dto.IdMedico,
                Nombre = dto.Nombre.Trim(),
                Apellido = dto.Apellido.Trim(),
                Dni = new DocumentoIdentidad(dto.NumeroDocumento.Trim()),
                FechaNacimiento = dto.FechaNacimiento,
                Genero = EnumeracionMapperNegocio.MapearGenero(dto.Genero),
                Telefono = new Telefono(dto.Telefono.Trim()),
                Matricula = dto.Matricula.Trim(),
                Especialidades = especialidades,
                Usuario = new Usuario { IdUsuario = dto.IdUsuario },
                Estado = EnumeracionMapperNegocio.MapearEstadoEntidad(dto.Estado),
                FechaModificacion = DateTime.Now
            };
        }




        public static MedicoDetalleDto MapearADetalleDto(Medico medico)
        {
            List<int> ids = new List<int>();
            List<string> nombres = new List<string>();
            if (medico.Especialidades != null)
            {
                foreach (var esp in medico.Especialidades)
                {
                    ids.Add(esp.IdEspecialidad);
                    nombres.Add(esp.Nombre);
                }
            }

            string generoTexto = string.Empty;
            switch (medico.Genero)
            {
                case SGTO.Dominio.Enums.Genero.Masculino:
                    generoTexto = "Masculino";
                    break;
                case SGTO.Dominio.Enums.Genero.Femenino:
                    generoTexto = "Femenino";
                    break;
                case SGTO.Dominio.Enums.Genero.Otro:
                    generoTexto = "Otro";
                    break;
                case SGTO.Dominio.Enums.Genero.PrefiereNoDecir: 
                    generoTexto = "Prefiere no decir";
                    break;
                default:
                    generoTexto = medico.Genero.ToString();
                    break;
            }

            return new MedicoDetalleDto
            {
                IdMedico = medico.IdMedico,
                NombreCompleto = $"{medico.Nombre} {medico.Apellido}".Trim(),
                NumeroDocumento = medico.Dni != null ? medico.Dni.Numero : string.Empty,
                FechaNacimiento = medico.FechaNacimiento,
                Telefono = medico.Telefono != null ? medico.Telefono.Numero : string.Empty,
                Email = medico.Email != null ? medico.Email.Valor : "Sin email",
                Estado = medico.Estado.ToString(),

                Genero = generoTexto,

                Matricula = medico.Matricula,
                FechaIncorporacion = medico.FechaAlta,
                NombreUsuario = medico.Usuario != null ? medico.Usuario.NombreUsuario : "Sin usuario",

                IdEspecialidades = ids,
                Especialidades = nombres,

                CoberturasAceptadas = new List<string>(),
                CantidadPacientesAtendidos = 0,
                HistorialTurnos = new List<SGTO.Comun.DTOs.TurnoHistorialDto>()
            };
        }
        public static MedicoEdicionDto MapearAEdicionDto(Medico medico)
        {
            List<int> idEspecialidades = new List<int>();

            foreach (Especialidad especialidad in medico.Especialidades)
            {
                idEspecialidades.Add(especialidad.IdEspecialidad);
            }
            return new MedicoEdicionDto
            {
                IdUsuario = medico.Usuario.IdUsuario,
                IdMedico = medico.IdMedico,
                NumeroDocumento = medico.Dni.Numero,
                FechaNacimiento = medico.FechaNacimiento,
                Genero = medico.Genero.ToString(),
                Telefono = medico.Telefono.Numero,
                Matricula = medico.Matricula,
                IdEspecialidades = idEspecialidades,
                Estado = medico.Estado.ToString()
            };
        }

        public static MedicoListadoDto MapearADto(Medico entidad)
        {
            if (entidad == null)
                return null;

            List<string> nombresEspecialidades = new List<string>();

            if (entidad.Especialidades != null)
            {
                foreach (var esp in entidad.Especialidades)
                    nombresEspecialidades.Add(esp.Nombre);
            }

            string nombreCompleto = string.Empty;

            if (!string.IsNullOrWhiteSpace(entidad.Apellido) || !string.IsNullOrWhiteSpace(entidad.Nombre))
            {
                nombreCompleto = $"{entidad.Apellido}, {entidad.Nombre}".Trim(',', ' ');
            }

            MedicoListadoDto dto = new MedicoListadoDto
            {
                IdMedico = entidad.IdMedico,
                NombreCompleto = nombreCompleto,
                Dni = entidad.Dni != null ? entidad.Dni.Numero : string.Empty,
                Matricula = entidad.Matricula,
                Telefono = entidad.Telefono != null ? entidad.Telefono.Numero : string.Empty,
                Estado = entidad.Estado.ToString(),
                NombresEspecialidades = nombresEspecialidades
            };

            return dto;
        }

        public static List<MedicoListadoDto> MapearAListado(List<Medico> medicos)
        {
            List<MedicoListadoDto> medicosDtos = new List<MedicoListadoDto>();
            foreach (Medico medico in medicos)
            {
                medicosDtos.Add(MapearADto(medico));
            }
            return medicosDtos;
        }



    }
}