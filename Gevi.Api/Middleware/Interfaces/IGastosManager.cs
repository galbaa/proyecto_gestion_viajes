using Gevi.Api.Models;
using Gevi.Api.Models.Requests;
using Gevi.Api.Models.Responses;
using System.Collections.Generic;

namespace Gevi.Api.Middleware.Interfaces
{
    public interface IGastosManager
    {
        HttpResponse<GastoResponse> NuevoGasto(GastoRequest request);
        HttpResponse<GastoResponse> ValidarGasto(ValidacionRequest request);
        HttpResponse<List<GastoResponse>> Pendientes();

    }
}
