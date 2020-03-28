using Gevi.Api.Models;
using System.Collections.Generic;

namespace Gevi.Api.Middleware.Interfaces
{
    public interface IUsuariosManager
    {
        HttpResponse<List<UsuarioResponse>> Todos();
        HttpResponse<UsuarioResponse> NuevoUsuario(UsuarioRequest request);
        HttpResponse<UsuarioResponse> ModificarUsuario(UsuarioRequest request);
        HttpResponse<UsuarioResponse> CambiarContrasenia(UsuarioRequest request);
    }
}
