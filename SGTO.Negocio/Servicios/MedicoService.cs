using SGTO.Datos.Repositorios;
using SGTO.Negocio.DTOs;
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


        public void Crear(MedicoCrearDto nuevoMedico) { }
    }
}
