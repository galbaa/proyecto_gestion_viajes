using Gevi.Api.Models;
using Gevi.Api.Models.Requests;
using Gevi.Api.Models.Responses;

namespace Gevi.Api.Middleware.Interfaces
{
    public interface IGastosManager
    {
        HttpResponse<GastoResponse> NuevoGasto(GastoRequest request);
        HttpResponse<GastoResponse> ValidarGasto(GastoRequest request);
    }
}
