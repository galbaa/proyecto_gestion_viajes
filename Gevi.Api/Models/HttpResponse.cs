using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Gevi.Api.Models
{
    public class HttpResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public ApiResponse<T> ApiResponse{ get; set; }
    }
}