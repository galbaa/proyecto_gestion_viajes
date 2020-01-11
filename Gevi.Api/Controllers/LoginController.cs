using System.Data;
using System.Linq;
using System.Net;
using System.Web.Http;
using Gevi.Api.Models;
using Gevi.Api.Middleware.TokenGeneration;
using Gevi.Api.Middleware;

namespace Gevi.Api.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("login")]
    public class LoginController : ApiController
    {
        //private GeviApiContext db = new GeviApiContext();

        //[HttpPost]
        //[Route("standard")]
        //public IHttpActionResult Login(LoginRequest request)
        //{
        //    if (request == null)
        //        throw new HttpResponseException(HttpStatusCode.BadRequest);
 
        //    var encryptionManager = new EncryptionManager();
        //    var password = encryptionManager.Encryptdata(request.Password);

        //    var user = db.Usuarios.Where(u => u.Email.Equals(request.Username)
        //                && u.Contrasenia.Equals(password)).FirstOrDefault();
            
        //    if (user != null)
        //    {
        //        var token = TokenGenerator.GenerateTokenJwt(request.Username);
        //        return this.Content(HttpStatusCode.OK, new HttpResponse<UsuarioResponse>()
        //        {
        //            StatusCode = 200,
        //            ApiResponse = new ApiResponse<UsuarioResponse>()
        //            {
        //                Data = new UsuarioResponse()
        //                {
        //                    Id = user.Id,
        //                    Email = user.Email,
        //                    Nombre = user.Nombre,
        //                    FechaRegistro = user.FechaRegistro,
        //                    Token = token
        //                },
        //                Error = null
        //            }
        //        });
        //    }
        //    else
        //    {
        //        return this.Content(HttpStatusCode.Unauthorized, new HttpResponse<Error>()
        //        {
        //            StatusCode = 401,
        //            ApiResponse = new ApiResponse<Error>()
        //            {
        //                Data = null,
        //                Error = new Error("Login invalido. Verifique las credenciales.")
        //            }
        //        });
        //    }
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}