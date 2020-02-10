using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gevi.Api.Models
{
    public class UsuarioResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public bool EsEmpleado { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}