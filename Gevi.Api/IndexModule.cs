using Gevi.Api.Models;
using Nancy;
using Nancy.ModelBinding;
using Gevi.Api.Middleware.Interfaces;

namespace Gevi.Api
{
    public class IndexModule : NancyModule
    {
        /*
        public IndexModule(ILoginManager loginManager)
        {
            Post("/standard", _ =>
            {
                var loginRequest = this.Bind<LoginRequest>();
                var loginResponse = loginManager.LoginStandard(loginRequest.Username, loginRequest.Password, Context);
                return Negotiate
                    .WithContentType("application/json")
                    .WithStatusCode(loginResponse.StatusCode)
                    .WithModel(loginResponse.ApiResponse);
            });
        }
        */
    }
}