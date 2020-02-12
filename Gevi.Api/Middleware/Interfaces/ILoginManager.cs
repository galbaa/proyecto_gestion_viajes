using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Gevi.Api.Models;
using Gevi.Api.Models.Responses;

namespace Gevi.Api.Middleware.Interfaces
{
    public interface ILoginManager
    {
        HttpResponse<LoginResponse> Login(string username, string password);
    }
}
