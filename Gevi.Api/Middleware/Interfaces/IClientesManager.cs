using Gevi.Api.Models;
using Gevi.Api.Models.Requests;
using Gevi.Api.Models.Responses;
using System.Collections.Generic;

namespace Gevi.Api.Middleware.Interfaces
{
    public interface IClientesManager
    {
        HttpResponse<ClienteResponse> NuevoCliente(ClienteRequest request);
        HttpResponse<ClienteResponse> BorrarCliente(ClienteRequest request);
        HttpResponse<ClienteResponse> ModificarCliente(ClienteRequest request);
        HttpResponse<ClienteResponse> BuscarCliente(ClienteRequest request);
        HttpResponse<List<ClienteResponse>> Todos();
    }
}
