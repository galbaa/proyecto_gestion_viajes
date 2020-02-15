using Gevi.Api.Models.Responses;
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
        public string EmpleadoNombre { get; set; }
        public List<GastoResponse> Gastos { get; set; }
        public string Proyecto { get; set; }
        public string ClienteProyectoNombre { get; set; }
    }
}