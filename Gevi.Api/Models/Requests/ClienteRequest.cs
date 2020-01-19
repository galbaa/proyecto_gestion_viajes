using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gevi.Api.Models.Requests
{
    public class ClienteRequest
    {
        public string Nombre { get; set; }
        public string Pais { get; set; }
        public string Tipo { get; set; }
    }
}