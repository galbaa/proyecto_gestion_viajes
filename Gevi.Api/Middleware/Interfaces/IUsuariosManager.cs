using Gevi.Api.Models;

namespace Gevi.Api.Middleware.Interfaces
{
    public interface IUsuariosManager
    {
        HttpResponse<UsuarioResponse> NuevoUsuario(UsuarioRequest request);
        HttpResponse<UsuarioResponse> ModificarUsuario(UsuarioRequest request);
        HttpResponse<UsuarioResponse> CambiarContrasenia(UsuarioRequest request);
    }
}
