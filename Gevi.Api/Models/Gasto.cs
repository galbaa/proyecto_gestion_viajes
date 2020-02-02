using System;

namespace Gevi.Api.Models
{
    public class Gasto
    {
        public int Id { get; set; }
        public Estado Estado { get; set; }
        public string Moneda { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
        public Viaje Viaje { get; set; }
        public TipoGasto Tipo { get; set; }
    }
}