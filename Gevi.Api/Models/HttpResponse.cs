using Nancy;

namespace Gevi.Api.Models
{
    public class HttpResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public ApiResponse<T> ApiResponse{ get; set; }
    }
}