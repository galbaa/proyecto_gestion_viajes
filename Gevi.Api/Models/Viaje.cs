using System;
using System.Collections.Generic;

namespace Gevi.Api.Models
{
    public enum Estado
    {
        PENDIENTE_APROBACION,
        APROBADO
    }

    public class Viaje
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public Estado Estado { get; set; }
        public Empleado Empleado { get; set; }
        public List<Gasto> Gastos { get; set; }
        public Proyecto Proyecto { get; set; }

        public Viaje()
        {
            
        }

        public Viaje(DateTime fInicio, DateTime fFin, Estado estado, Empleado empleado)
        {
            this.FechaInicio = fInicio;
            this.FechaFin = fFin;
            this.Estado = estado;
            this.Empleado = empleado;
        }
    }
}