using Gevi.Api.Middleware.Interfaces;
using Gevi.Api.Models;
using Gevi.Api.Models.Requests;
using Gevi.Api.Models.Responses;
using Nancy;
using System.Data.Entity.Infrastructure;

namespace Gevi.Api.Middleware
{
    public class GastosManager : IGastosManager
    {
        public HttpResponse<GastoResponse> NuevoGasto(GastoRequest request)
        {
            if (request == null)
                return newHttpErrorResponse(new Error("El gasto que se intenta ingresar es invalido."));

            using (var db = new GeviApiContext())
            {
                var nuevo = new Gasto()
                {
                    Estado = request.Estado,
                    Fecha = request.Fecha,
                    Moneda = request.Moneda,
                    Tipo = request.Tipo,
                    Total = request.Total
                };

                try
                {
                    db.Gastos.Add(nuevo);
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    return newHttpErrorResponse(new Error("Error al ingresar nuevo gasto."));
                }

                var response = new GastoResponse()
                {
                    Id = nuevo.Id,
                    Estado = nuevo.Estado,
                    Fecha = nuevo.Fecha,
                    Moneda = nuevo.Moneda,
                    Tipo = nuevo.Tipo,
                    Total = nuevo.Total
                };

                return newHttpResponse(response);
            }
        }

        public HttpResponse<GastoResponse> ValidarGasto(GastoRequest request)
        {
            return null;
        }

        private HttpResponse<GastoResponse> newHttpResponse(GastoResponse response)
        {
            return new HttpResponse<GastoResponse>()
            {
                StatusCode = HttpStatusCode.OK,
                ApiResponse = new ApiResponse<GastoResponse>()
                {
                    Data = response,
                    Error = null
                }
            };
        }

        private HttpResponse<GastoResponse> newHttpErrorResponse(Error error)
        {
            return new HttpResponse<GastoResponse>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ApiResponse = new ApiResponse<GastoResponse>()
                {
                    Data = null,
                    Error = error
                }
            };
        }
    }
}