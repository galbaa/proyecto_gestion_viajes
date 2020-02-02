using System;

namespace Gevi.Api.Models.Requests
{
    public class ProyectoRequest
    {
        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public string ClienteNombre { get; set; }
    }
}