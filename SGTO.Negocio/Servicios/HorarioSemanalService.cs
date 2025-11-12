using SGTO.Datos.Repositorios;
using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs.Medicos;
using SGTO.Negocio.Mappers;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SGTO.Negocio.Servicios
{
    public class HorarioSemanalService
    {
        private readonly HorarioSemanalRepositorio _repositorioHorario;

        public HorarioSemanalService()
        {
            _repositorioHorario = new HorarioSemanalRepositorio();
        }

        public List<HorarioSemanalDto> ObtenerPorMedico(int idMedico)
        {
            try
            {
                List<HorarioSemanalMedico> horariosEntidad = _repositorioHorario.ObtenerPorMedico(idMedico);
                return HorarioSemanalMapper.MapearListaADto(horariosEntidad);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error en HorarioSemanalService.ObtenerPorMedico: " + ex.Message);
                throw;
            }
        }

    }
}
