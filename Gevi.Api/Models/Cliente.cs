using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gevi.Api.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Pais { get; set; }
        public string Moneda { get; set; }
        public TipoCliente Tipo { get; set; }
    }
}