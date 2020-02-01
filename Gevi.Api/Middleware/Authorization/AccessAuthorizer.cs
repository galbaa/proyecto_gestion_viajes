using Gevi.Api.Middleware.Interfaces;
using Nancy;
using System.Configuration;

namespace Gevi.Api.Middleware.Authorization
{
    public class AccessAuthorizer : IAccessAuthorizer
    {
        private readonly IHeaderParser _headerParser;

        public AccessAuthorizer(IHeaderParser headerParser)
        {
            _headerParser = headerParser;
        }
        public Response Authorized(NancyContext context)
        {
            var apiKey = _headerParser.GetApiKey(context);

            if (apiKey == ConfigurationManager.AppSettings["API_KEY"])
                return null;

            return new Response { StatusCode = HttpStatusCode.Unauthorized };
        }
    }
}