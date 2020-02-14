using Gevi.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gevi.Api.Middleware.Interfaces
{
    public interface IUsuariosManager
    {
        HttpResponse<UsuarioResponse> NuevoUsuario(UsuarioRequest request);
        HttpResponse<UsuarioResponse> ModificarUsuario(UsuarioRequest request);
        HttpResponse<UsuarioResponse> CambiarContrasenia(UsuarioRequest request);
    }
}
