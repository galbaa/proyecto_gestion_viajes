using System;

namespace Gevi.Api.Models
{
    public class Gasto
    {
        public int Id { get; set; }
        public string Estado { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
    }
}