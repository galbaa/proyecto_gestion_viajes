using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gevi.Api.Models.Responses
{
    public class EstadisticasResponse
    {
        public decimal TotalTransporte { get; set; }
        public decimal TotalGastronomico { get; set; }
        public decimal TotalTelefonia { get; set; }
        public decimal TotalOtros { get; set; }
    }
}