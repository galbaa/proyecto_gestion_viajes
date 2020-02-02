using Gevi.Api.Middleware.Interfaces;
using Gevi.Api.Models;
using Gevi.Api.Models.Requests;
using Gevi.Api.Models.Responses;
using Nancy;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

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

        public HttpResponse<GastoResponse> ValidarGasto(ValidacionRequest request)
        {
            if (request == null)
                return newHttpErrorResponse(new Error("El gasto que se intenta validar es invalido"));

            using (var db = new GeviApiContext())
            {
                var gasto = db.Gastos
                                .Where(g => g.Id == request.Id)
                                .Include(g => g.Tipo)
                                .Include(g => g.Viaje)
                                .FirstOrDefault();

                if (gasto != null)
                {
                    gasto.Estado = request.Estado;
                    db.Entry(gasto).State = EntityState.Modified;
                    db.SaveChanges();

                    var response = new GastoResponse()
                    {
                        Id = gasto.Id,
                        Moneda = gasto.Moneda,
                        Estado = gasto.Estado,
                        Fecha = gasto.Fecha,
                        Tipo = gasto.Tipo,
                        Total = gasto.Total,
                        ViajeId = gasto.Viaje == null ? 0 : gasto.Viaje.Id
                    };

                    return newHttpResponse(response);
                }

                return newHttpErrorResponse(new Error("No existe el gasto"));
            }
        }
        public HttpResponse<List<GastoResponse>> Pendientes()
        {
            using (var db = new GeviApiContext())
            {
                var gastosPendientes = db.Gastos
                                            .Where(g => g.Estado == Estado.PENDIENTE_APROBACION)
                                            .ToList();
                var response = new List<GastoResponse>();

                foreach (var g in gastosPendientes)
                {
                    var nuevo = new GastoResponse()
                    {
                        Id = g.Id,
                        Estado = g.Estado,
                        Fecha = g.Fecha,
                        Moneda = g.Moneda,
                        Tipo = g.Tipo,
                        Total = g.Total,
                        ViajeId = g.Viaje == null ? 0 : g.Viaje.Id
                    };

                    response.Add(nuevo);
                }

                return newHttpListResponse(response);
            }
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

        private HttpResponse<List<GastoResponse>> newHttpListResponse(List<GastoResponse> response)
        {
            return new HttpResponse<List<GastoResponse>>()
            {
                StatusCode = HttpStatusCode.OK,
                ApiResponse = new ApiResponse<List<GastoResponse>>()
                {
                    Data = response,
                    Error = null
                }
            };
        }
    }
}