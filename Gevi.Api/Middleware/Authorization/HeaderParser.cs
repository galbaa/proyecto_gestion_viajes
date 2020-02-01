using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gevi.Api.Middleware.Interfaces;
using Nancy;

namespace Gevi.Api.Middleware.Authorization
{
    public class HeaderParser : IHeaderParser
    {
        private const string ApiKey = "x-gevi-api-key";

        public string GetApiKey(NancyContext context)
        {
            return GetRequestHeader(context.Request.Headers, ApiKey);
        }

        private static string GetRequestHeader(RequestHeaders headers, string headerName)
        {
            if (headers == null || !headers.Keys.Contains(headerName))
                return null;

            return headers[headerName].FirstOrDefault();
        }
    }
}