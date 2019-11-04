using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gevi.Api.Models
{
    public class Empleado : Usuario
    {
        public List<Viaje> Viajes { get; set; }
    }
}