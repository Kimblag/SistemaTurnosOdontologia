using SGTO.Comun.Validacion;
using SGTO.Datos.Repositorios;
using SGTO.Dominio.Entidades;
using SGTO.Dominio.Enums;
using SGTO.Dominio.ObjetosValor;
using SGTO.Negocio.DTOs.Medicos;
using SGTO.Negocio.Excepciones;
using SGTO.Negocio.Mappers;
using System;
using System.Collections.Generic;

namespace SGTO.Negocio.Servicios
{
    public class MedicoService
    {

        private readonly MedicoRepositorio _repositorioMedico;

        public MedicoService()
        {
            _repositorioMedico = new MedicoRepositorio();
        }

        public List<MedicoListadoDto> Listar(string estado = null)
        {
            try
            {
                return MedicoMapper.MapearAListado(_repositorioMedico.Listar(estado));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
