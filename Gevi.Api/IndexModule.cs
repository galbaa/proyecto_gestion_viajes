using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Gevi.Api.Models;
using Nancy.ModelBinding;
using Gevi.Api.Middleware.Interfaces;

namespace Gevi.Api
{
    public class IndexModule : NancyModule
    {
        public IndexModule(ILoginManager loginManager)
        {
            Post["login/standard"] = parameters =>
            {
                var loginRequest = this.Bind<LoginRequest>();
                var loginResponse = loginManager.Login(loginRequest.Username, loginRequest.Password, Context);

                return Negotiate
                    .WithContentType("application/json")
                    .WithStatusCode(loginResponse.StatusCode)
                    .WithModel(loginResponse.ApiResponse);
            };
        }
    }
}