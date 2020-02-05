using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gevi.Api.Models.Requests
{
    public class EstadisticasRequest
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string ClienteNombre { get; set; }
        public int EmpleadoId { get; set; }
    }
}