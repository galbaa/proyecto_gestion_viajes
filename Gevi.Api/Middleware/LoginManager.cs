using System.Linq;
using Gevi.Api.Middleware.Interfaces;
using Gevi.Api.Models;
using Nancy;

namespace Gevi.Api.Middleware
{
    public class LoginManager : ILoginManager
    {
        public HttpResponse<UsuarioResponse> Login(string username, string password)
        {
            var encryptionManager = new EncryptionManager();
            var pass = encryptionManager.Encryptdata(password);

            using (var db = new GeviApiContext())
            {
                var user = db.Usuarios.Where(u => u.Email.Equals(username)
                                && u.Contrasenia.Equals(pass)).FirstOrDefault();

                if(user != null)
                {
                    return newHttpResponse(new UsuarioResponse()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FechaRegistro = user.FechaRegistro,
                        Nombre = user.Nombre
                    });
                }
                else
                {
                    return newHttpErrorResponse(new Error("Login invalido. Verifique las credenciales."));
                }
            }
        }

        private HttpResponse<UsuarioResponse> newHttpResponse(UsuarioResponse response)
        {
            return new HttpResponse<UsuarioResponse>()
            {
                StatusCode = HttpStatusCode.OK,
                ApiResponse = new ApiResponse<UsuarioResponse>()
                {
                    Data = response,
                    Error = null
                }
            };
        }

        private HttpResponse<UsuarioResponse> newHttpErrorResponse(Error error)
        {
            return new HttpResponse<UsuarioResponse>()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                ApiResponse = new ApiResponse<UsuarioResponse>()
                {
                    Data = null,
                    Error = error
                }
            };
        }
    }
}