using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gevi.Api.Models
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public Error Error { get; set; }
    }
}