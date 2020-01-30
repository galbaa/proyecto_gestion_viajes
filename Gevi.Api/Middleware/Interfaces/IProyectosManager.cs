using Gevi.Api.Models;
using Gevi.Api.Models.Requests;
using Gevi.Api.Models.Responses;
using System.Collections.Generic;

namespace Gevi.Api.Middleware.Interfaces
{
    public interface IProyectosManager
    {
        HttpResponse<ProyectoResponse> NuevoProyecto(ProyectoRequest request);
        HttpResponse<ProyectoResponse> ModificarProyecto(ProyectoRequest request);
        HttpResponse<ProyectoResponse> BorrarProyecto(ProyectoRequest request);
        HttpResponse<List<ProyectoResponse>> Todos();
    }
}
