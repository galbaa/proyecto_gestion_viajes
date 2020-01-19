using System;
using System.Collections.Generic;

namespace Gevi.Api.Models
{
    public class ViajeResponse
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public Estado Estado { get; set; }
        public int EmpleadoId { get; set; }
        public List<Gasto> Gastos { get; set; }
        public Proyecto Proyecto { get; set; }
    }
}