using System.Linq;
using Gevi.Api.Models;
using Gevi.Api.Middleware.Interfaces;
using Nancy;
using Gevi.Api.Middleware.TokenGeneration;

namespace Gevi.Api.Middleware
{
    public class LoginManager
    {
        /*public HttpResponse<UsuarioResponse> LoginStandard(string username, string password, NancyContext context)
        {
            using (var db = new GeviApiContext())
            {
                var usu = db.Usuarios.Where(u => u.Email.Equals(username) && u.Contrasenia.Equals(password))
                            .FirstOrDefault();

                if (usu != null)
                {
                    var token = TokenGenerator.GenerateTokenJwt(username);
                    var loginResponse = new UsuarioResponse()
                    {
                        Id = usu.Id,
                        Email = usu.Email,
                        Nombre = usu.Nombre,
                        FechaRegistro = usu.FechaRegistro,
                        Token = token
                    };
                    return NewHttpResponse(loginResponse);
                }
                else
                {
                    return NewHttpErrorResponse(new Error("Login invalido. Verifique las credenciales"));
                }
            }
        }

        private HttpResponse<UsuarioResponse> NewHttpResponse(UsuarioResponse dataResponse)
        {
            return new HttpResponse<UsuarioResponse>
            {
                StatusCode = HttpStatusCode.OK,
                ApiResponse = new ApiResponse<UsuarioResponse>
                {
                    Data = dataResponse,
                    Error = null
                }
            };
        }

        private HttpResponse<UsuarioResponse> NewHttpErrorResponse(Error error)
        {
            return new HttpResponse<UsuarioResponse>
            {
                StatusCode = HttpStatusCode.Unauthorized,
                ApiResponse = new ApiResponse<UsuarioResponse>
                {
                    Data = null,
                    Error = error
                }
            };
        }
    }*/
}
