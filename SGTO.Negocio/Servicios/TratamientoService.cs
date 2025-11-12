// using SGTO.Datos.Infraestructura; // Ya no se necesita
using SGTO.Datos.Repositorios;
using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums; 
using SGTO.Negocio.DTOs;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Mappers;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SGTO.Negocio.Servicios
{
    public class TratamientoService
    {
        private readonly TratamientoRepositorio _repositorio;

        public TratamientoService()
        {
            _repositorio = new TratamientoRepositorio();
        }


        public List<TratamientoDto> ObtenerTodosDto(string estado = null)
        {
            List<Tratamiento> tratamientos = _repositorio.Listar(estado);
            List<TratamientoDto> dtos = TratamientoMapper.MapearListaADto(tratamientos);
            return dtos;
        }

        public List<TratamientoDto> Listar(string estado = null)
        {
            try
            {
                return TratamientoMapper.MapearListaADto(_repositorio.Listar(estado));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR: " + ex.Message);
                throw;
            }
        }

    }
}