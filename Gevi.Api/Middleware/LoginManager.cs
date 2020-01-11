using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gevi.Api.Middleware.Interfaces;
using Gevi.Api.Models;
using Nancy;

namespace Gevi.Api.Middleware
{
    public class LoginManager : ILoginManager
    {
        public HttpResponse<UsuarioResponse> Login(string username, string password, NancyContext context)
        {
            return new HttpResponse<UsuarioResponse>()
            {
                ApiResponse = null,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}