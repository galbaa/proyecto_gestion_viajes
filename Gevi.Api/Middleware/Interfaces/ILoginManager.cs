using Gevi.Api.Models;
using Nancy;

namespace Gevi.Api.Middleware.Interfaces
{
    public interface ILoginManager
    {
        HttpResponse<UsuarioResponse> LoginStandard(string username, string password, NancyContext context);
    }
}
