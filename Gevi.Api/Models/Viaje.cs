using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gevi.Api.Models
{
    public class Viaje
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }
        public Empleado Empleado { get; set; }
        public List<Gasto> Gastos { get; set; }
        public Proyecto Proyecto { get; set; }

        public Viaje()
        {

        }

        public Viaje(DateTime fInicio, DateTime fFin, string estado, Empleado empleado)
        {
            this.FechaInicio = fInicio;
            this.FechaFin = fFin;
            this.Estado = estado;
            this.Empleado = empleado;
        }
    }
}