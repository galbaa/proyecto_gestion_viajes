using Gevi.Api.Middleware.Interfaces;
using Gevi.Api.Models;
using Gevi.Api.Models.Requests;
using Gevi.Api.Models.Responses;
using Nancy;
using System;
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
                var empleado = db.Usuarios
                                    .OfType<Empleado>()
                                    .Where(u => u is Empleado && u.Id == request.EmpleadoId)
                                    .Include(u => u.Viajes)
                                    .FirstOrDefault();
                if (empleado != null)
                {
                    var viaje = db.Viajes
                                    .Where(v => v.Id == request.ViajeId)
                                    .Include(v => v.Proyecto)
                                    .Include(v => v.Gastos)
                                    .Include(v => v.Empleado)
                                    .FirstOrDefault();
                    if (viaje != null && 
                        request.Fecha >= viaje.FechaInicio && 
                        request.Fecha <= viaje.FechaFin)
                    {
                        var tipo = db.TipoGastos
                                        .Where(t => t.Nombre.Equals(request.Tipo))
                                        .FirstOrDefault();

                        var moneda = db.Monedas
                                        .Where(m => m.Nombre.Equals(request.MonedaNombre))
                                        .FirstOrDefault();

                        var nuevo = new Gasto()
                        {
                            Estado = request.Estado,
                            Fecha = request.Fecha,
                            Moneda = moneda,
                            Tipo = tipo,
                            Empleado = empleado,
                            Viaje = viaje,
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
                            Moneda = nuevo.Moneda?.Nombre,
                            Tipo = nuevo.Tipo?.Nombre,
                            ViajeId = nuevo.Viaje == null ? 0 : nuevo.Viaje.Id,
                            Proyecto = nuevo.Viaje?.Proyecto?.Nombre,
                            Empleado = nuevo.Empleado?.Nombre,
                            Total = nuevo.Total
                        };

                        return newHttpResponse(response);
                    }
                    else
                    {
                        if (viaje == null)
                            return newHttpErrorResponse(new Error("El viaje no existe"));
                        else
                            return newHttpErrorResponse(new Error("La fecha del gasto debe pertenecer al viaje"));
                    }
                }

                return newHttpErrorResponse(new Error("El empleado no existe"));
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
                                .Include(g => g.Empleado)
                                .Include(g => g.Viaje.Proyecto)
                                .Include(g => g.Moneda)
                                .FirstOrDefault();

                if (gasto != null)
                {
                    gasto.Estado = request.Estado;
                    db.Entry(gasto).State = EntityState.Modified;
                    db.SaveChanges();

                    var response = new GastoResponse()
                    {
                        Id = gasto.Id,
                        Moneda = gasto.Moneda?.Nombre,
                        Estado = gasto.Estado,
                        Fecha = gasto.Fecha,
                        Tipo = gasto.Tipo?.Nombre,
                        Total = gasto.Total,
                        Empleado = gasto.Empleado?.Nombre,
                        Proyecto = gasto.Viaje?.Proyecto?.Nombre,
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
                                            .Include(g => g.Viaje.Proyecto)
                                            .Include(g => g.Tipo)
                                            .Include(g => g.Empleado)
                                            .Include(g => g.Moneda)
                                            .ToList();

                var response = new List<GastoResponse>();

                foreach (var g in gastosPendientes)
                {
                    var nuevo = new GastoResponse()
                    {
                        Id = g.Id,
                        Estado = g.Estado,
                        Fecha = g.Fecha,
                        Moneda = g.Moneda?.Nombre,
                        Tipo = g.Tipo?.Nombre,
                        Total = g.Total,
                        Empleado = g.Empleado?.Nombre,
                        Proyecto = g.Viaje?.Proyecto?.Nombre,
                        ViajeId = g.Viaje == null ? 0 : g.Viaje.Id
                    };

                    response.Add(nuevo);
                }

                return newHttpListResponse(response);
            }
        }
        public HttpResponse<EstadisticasResponse> GetEstadisticas(EstadisticasRequest request)
        {
            if (request == null)
                return newHttpEstadisticasErrorResponse(new Error("Los parametros pasados son invalidos."));

            using (var db = new GeviApiContext())
            {
                var totalTransporte = db.Gastos
                                            .Where(g => g.Estado == Estado.APROBADO &&
                                                        (!String.IsNullOrEmpty(request.ClienteNombre) && 
                                                            (g.Viaje != null) && (g.Viaje.Proyecto != null) &&
                                                            (g.Viaje.Proyecto.Cliente != null) ? g.Viaje.Proyecto.Cliente.Nombre.Equals(request.ClienteNombre) : true) &&
                                                        (request.EmpleadoId > 0 && (g.Empleado != null) ? g.Empleado.Id == request.EmpleadoId : true) &&
                                                        (!request.FechaInicio.Equals(DateTime.MinValue) ? g.Fecha >= request.FechaInicio : true) &&
                                                        (!request.FechaFin.Equals(DateTime.MinValue) ? g.Fecha <= request.FechaFin : true))
                                            .Where(g => g.Tipo.Nombre.Equals("TRANSPORTE"))
                                            .Include(g => g.Empleado)
                                            .Include(g => g.Tipo)
                                            .Include(g => g.Viaje)
                                            .ToList()
                                            .Select(g => g.Total)
                                            .Sum();

                var totalGastronomico = db.Gastos
                                            .Where(g => g.Estado == Estado.APROBADO &&
                                                        (!String.IsNullOrEmpty(request.ClienteNombre) &&
                                                            (g.Viaje != null) && (g.Viaje.Proyecto != null) &&
                                                            (g.Viaje.Proyecto.Cliente != null) ? g.Viaje.Proyecto.Cliente.Nombre.Equals(request.ClienteNombre) : true) &&
                                                        (request.EmpleadoId > 0 && (g.Empleado != null) ? g.Empleado.Id == request.EmpleadoId : true) &&
                                                        (!request.FechaInicio.Equals(DateTime.MinValue) ? g.Fecha >= request.FechaInicio : true) &&
                                                        (!request.FechaFin.Equals(DateTime.MinValue) ? g.Fecha <= request.FechaFin : true))
                                            .Where(g => g.Tipo.Nombre.Equals("GASTRONOMICO"))
                                            .Include(g => g.Empleado)
                                            .Include(g => g.Tipo)
                                            .Include(g => g.Viaje)
                                            .ToList()
                                            .Select(g => g.Total)
                                            .Sum();

                var totalTelefonia = db.Gastos
                                            .Where(g => g.Estado == Estado.APROBADO &&
                                                        (!String.IsNullOrEmpty(request.ClienteNombre) &&
                                                            (g.Viaje != null) && (g.Viaje.Proyecto != null) &&
                                                            (g.Viaje.Proyecto.Cliente != null) ? g.Viaje.Proyecto.Cliente.Nombre.Equals(request.ClienteNombre) : true) &&
                                                        (request.EmpleadoId > 0 && (g.Empleado != null) ? g.Empleado.Id == request.EmpleadoId : true) &&
                                                        (!request.FechaInicio.Equals(DateTime.MinValue) ? g.Fecha >= request.FechaInicio : true) &&
                                                        (!request.FechaFin.Equals(DateTime.MinValue) ? g.Fecha <= request.FechaFin : true))
                                            .Where(g => g.Tipo.Nombre.Equals("TELEFONIA"))
                                            .Include(g => g.Empleado)
                                            .Include(g => g.Tipo)
                                            .Include(g => g.Viaje)
                                            .ToList()
                                            .Select(g => g.Total)
                                            .Sum();

                var totalOtros = db.Gastos
                                            .Where(g => g.Estado == Estado.APROBADO &&
                                                        (!String.IsNullOrEmpty(request.ClienteNombre) &&
                                                            (g.Viaje != null) && (g.Viaje.Proyecto != null) &&
                                                            (g.Viaje.Proyecto.Cliente != null) ? g.Viaje.Proyecto.Cliente.Nombre.Equals(request.ClienteNombre) : true) &&
                                                        (request.EmpleadoId > 0 && (g.Empleado != null) ? g.Empleado.Id == request.EmpleadoId : true) &&
                                                        (!request.FechaInicio.Equals(DateTime.MinValue) ? g.Fecha >= request.FechaInicio : true) &&
                                                        (!request.FechaFin.Equals(DateTime.MinValue) ? g.Fecha <= request.FechaFin : true))
                                            .Where(g => g.Tipo.Nombre.Equals("OTROS"))
                                            .Include(g => g.Empleado)
                                            .Include(g => g.Tipo)
                                            .Include(g => g.Viaje)
                                            .ToList()
                                            .Select(g => g.Total)
                                            .Sum();


                var response = new EstadisticasResponse()
                {
                    TotalTransporte = totalTransporte,
                    TotalGastronomico = totalGastronomico,
                    TotalTelefonia = totalTelefonia,
                    TotalOtros = totalOtros
                };

                return newHttpEstadisticasResponse(response);
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
        private HttpResponse<EstadisticasResponse> newHttpEstadisticasResponse(EstadisticasResponse response)
        {
            return new HttpResponse<EstadisticasResponse>()
            {
                StatusCode = HttpStatusCode.OK,
                ApiResponse = new ApiResponse<EstadisticasResponse>()
                {
                    Data = response,
                    Error = null
                }
            };
        }
        private HttpResponse<EstadisticasResponse> newHttpEstadisticasErrorResponse(Error error)
        {
            return new HttpResponse<EstadisticasResponse>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ApiResponse = new ApiResponse<EstadisticasResponse>()
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