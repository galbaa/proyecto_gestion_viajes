using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gevi.Api.Models.Responses
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public bool EsEmpleado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public List<ClienteResponse> Clientes { get; set; }
        public List<TipoResponse> TipoClientes { get; set; }
        public List<Moneda> Monedas { get; set; }
        public List<TipoResponse> TipoGastos { get; set; }
    }
}