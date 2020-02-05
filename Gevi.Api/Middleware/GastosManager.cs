﻿using Gevi.Api.Middleware.Interfaces;
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

                var viaje = db.Viajes
                                .Where(v => v.Id == request.ViajeId)
                                .Include(v => v.Proyecto)
                                .Include(v => v.Gastos)
                                .Include(v => v.Empleado)
                                .FirstOrDefault();

                var tipo = db.TipoGastos
                                .Where(t => t.Nombre.Equals(request.Tipo))
                                .FirstOrDefault();

                var nuevo = new Gasto()
                {
                    Estado = request.Estado,
                    Fecha = request.Fecha,
                    Moneda = request.Moneda,
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
                    Moneda = nuevo.Moneda,
                    Tipo = nuevo.Tipo?.Nombre,
                    ViajeId = nuevo.Viaje == null ? 0 : nuevo.Viaje.Id,
                    EmpleadoNombre = nuevo.Empleado?.Nombre,
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
                                .Include(g => g.Empleado)
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
                        Tipo = gasto.Tipo?.Nombre,
                        Total = gasto.Total,
                        EmpleadoNombre = gasto.Empleado?.Nombre,
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
                                            .Include(g => g.Viaje)
                                            .Include(g => g.Tipo)
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
                        Tipo = g.Tipo?.Nombre,
                        Total = g.Total,
                        ViajeId = g.Viaje == null ? 0 : g.Viaje.Id
                    };

                    response.Add(nuevo);
                }

                return newHttpListResponse(response);
            }
        }

        /*public HttpResponse<EstadisticasResponse> GetEstadisticas(DateTime inicio, DateTime fin)
        {

        }*/

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