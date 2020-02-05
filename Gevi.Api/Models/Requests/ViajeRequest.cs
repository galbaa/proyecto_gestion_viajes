using System;
using System.Collections.Generic;

namespace Gevi.Api.Models
{
    public class ViajeRequest
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }
        public int EmpleadoId { get; set; }
        public List<Gasto> Gastos { get; set; }
        public int ProyectoId { get; set; }
    }
}