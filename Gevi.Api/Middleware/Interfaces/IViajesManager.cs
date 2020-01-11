using Gevi.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevi.Api.Middleware.Interfaces
{
    public interface IViajesManager
    {
        HttpResponse<Viaje> NuevoViaje(ViajeRequest viaje);
    }
}
