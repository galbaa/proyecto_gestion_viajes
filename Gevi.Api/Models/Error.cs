using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gevi.Api.Models
{
    public class Error
    {
        public string Mensaje { get; set; }

        public Error(string mensaje)
        {
            this.Mensaje = mensaje;
        }
    }
}