using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Gevi.Api.Models;

namespace Gevi.Api.Middleware.Interfaces
{
    public interface ILoginManager
    {
        HttpResponse<UsuarioResponse> Login(string username, string password, NancyContext context);
    }
}
