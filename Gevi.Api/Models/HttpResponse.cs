
namespace Gevi.Api.Models
{
    public class HttpResponse<T>
    {
        public int StatusCode { get; set; }
        public ApiResponse<T> ApiResponse{ get; set; }
    }
}