using Gevi.Api.Models;
using Gevi.Api.Models.Requests;
using Gevi.Api.Models.Responses;

namespace Gevi.Api.Middleware.Interfaces
{
    public interface IClientesManager
    {
        HttpResponse<ClienteResponse> NuevoCliente(ClienteRequest request);
        HttpResponse<ClienteResponse> BorrarCliente(ClienteRequest request);
    }
}
