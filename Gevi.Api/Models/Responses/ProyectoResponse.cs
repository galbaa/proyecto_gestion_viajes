using System;

namespace Gevi.Api.Models.Responses
{
    public class ProyectoResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaInicio { get; set; }
        public string Cliente { get; set; }
    }
}