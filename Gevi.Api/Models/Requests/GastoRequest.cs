using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gevi.Api.Models.Requests
{
    public class GastoRequest
    {
        public int Id { get; set; }
        public Estado Estado { get; set; }
        public string Moneda { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
        public int ViajeId { get; set; }
        public int EmpleadoId { get; set; }
        public string Tipo { get; set; }
    }
}