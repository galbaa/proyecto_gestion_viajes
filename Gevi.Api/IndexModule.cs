using Gevi.Api.Models;
using Nancy;
using Nancy.ModelBinding;

namespace Gevi.Api
{
    public class IndexModule : NancyModule
    {
        public IndexModule(ILoginManager loginManager) : base("/login")
        {
            
            Post("/standard",_ =>
            {
                var loginRequest = this.Bind<LoginRequest>();
                var loginResponse = loginManager.LoginStandard(loginRequest.Username, loginRequest.Password, Context);
                return Negotiate
                    .WithContentType("application/json")
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithModel(loginResponse.ApiResponse);
            });
        }
    }
}