using Gevi.Api.Models;

namespace Gevi.Api.Middleware.Interfaces
{
    public interface IViajesManager
    {
        HttpResponse<ViajeResponse> NuevoViaje(ViajeRequest viaje);
    }
}
