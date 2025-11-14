using System;
using System.Collections.Generic;
using System.Web;

namespace SGTO.UI.Webforms.Modelos.Medicos
{

    // usamos la clase Serializable porque así podemos mantener dentro del ViewStates los valores entre postbacks.
    // fuente: https://learn.microsoft.com/en-us/previous-versions/aspnet/bb386448(v=vs.100)
    // fuente: https://learn.microsoft.com/en-us/dotnet/api/system.web.ui.istateformatter.serialize?view=netframework-4.8.1

    [Serializable]
    public class HorarioSemanalItemUi
    {
        public string Inicio { get; set; }
        public string Fin { get; set; }

        public HorarioSemanalItemUi()
        {
        }

        public HorarioSemanalItemUi(string inicio, string fin)
        {
            Inicio = inicio;
            Fin = fin;
        }
    }
}