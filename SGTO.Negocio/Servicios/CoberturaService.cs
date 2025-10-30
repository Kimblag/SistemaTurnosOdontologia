using SGTO.Datos.Repositorios;
using SGTO.Dominio.Entidades;
using SGTO.Negocio.DTOs;
using SGTO.Negocio.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGTO.Negocio.Servicios
{
    public class CoberturaService
    {
        private readonly CoberturaRepositorio _repositorioCobertura;

        public CoberturaService()
        {
            _repositorioCobertura = new CoberturaRepositorio();
        }

        public List<CoberturaDto> Listar()
        {
            try
            {
                return CoberturaMapper.MapearListaADto(_repositorioCobertura.Listar());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
