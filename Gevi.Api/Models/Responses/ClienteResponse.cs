using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gevi.Api.Models.Responses
{
    public class ClienteResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<ProyectoResponse> Proyectos { get; set; }
        public TipoCliente Tipo { get; set; }
    }
}