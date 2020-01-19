using Gevi.Api.Middleware.Interfaces;
using Gevi.Api.Models;
using Nancy;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Gevi.Api.Middleware
{
    public class ViajesManager : IViajesManager
    {
        public HttpResponse<ViajeResponse> NuevoViaje(ViajeRequest viaje)
        {
            if (viaje == null)
                return newHttpErrorResponse(new Error("El viaje que se intenta ingresar es invalido."));

            using (var db = new GeviApiContext())
            {
                var empleado = (Empleado)db.Usuarios
                                    .Where(u => u is Empleado && u.Id == viaje.EmpleadoId)
                                    .FirstOrDefault();

                if (esValido(viaje, db, empleado))
                {
                    var response = new ViajeResponse()
                    {
                        EmpleadoId = empleado.Id,
                        Estado = Estado.PENDIENTE_APROBACION,
                        FechaInicio = viaje.FechaInicio,
                        FechaFin = viaje.FechaFin,
                        Gastos = null,
                        Proyecto = null
                    };

                    var nuevo = new Viaje()
                    {
                        Empleado = empleado,
                        Estado = Estado.PENDIENTE_APROBACION,
                        FechaInicio = viaje.FechaInicio,
                        FechaFin = viaje.FechaFin,
                        Gastos = null,
                        Proyecto = null
                    };

                    try
                    {
                        db.Viajes.Add(nuevo);
                        db.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        return newHttpErrorResponse(new Error("Ya existe un viaje con ese email."));
                    }

                    return newHttpResponse(response);
                }
                else
                {
                    return newHttpErrorResponse(new Error("Ya existe un viaje en esa fecha para el empleado."));
                }
            }
        }

        public HttpResponse<ViajeResponse> ValidarViaje(ValidacionRequest request)
        {
            if (request == null)
                return newHttpErrorResponse(new Error("La request es invalida."));

            using (var db = new GeviApiContext())
            {
                var viaje = db.Viajes
                    .Where(v => v.Id == request.ViajeId)
                    .Include(u => u.Empleado)
                    .FirstOrDefault();

                if (viaje != null)
                {
                    viaje.Estado = request.Estado;
                    db.Entry(viaje).State = EntityState.Modified;
                    db.SaveChanges();

                    var response = new ViajeResponse()
                    {
                        Id = viaje.Id,
                        EmpleadoId = viaje.Empleado.Id,
                        Estado = request.Estado,
                        FechaInicio = viaje.FechaInicio,
                        FechaFin = viaje.FechaFin,
                        Gastos = viaje.Gastos,
                        Proyecto = viaje.Proyecto
                    };
                    return newHttpResponse(response);
                }
                return newHttpErrorResponse(new Error("No existe el viaje"));
            }
        }

        private HttpResponse<ViajeResponse> newHttpResponse(ViajeResponse response)
        {
            return new HttpResponse<ViajeResponse>()
            {
                StatusCode = HttpStatusCode.OK,
                ApiResponse = new ApiResponse<ViajeResponse>()
                {
                    Data = response,
                    Error = null
                }
            };
        }

        private HttpResponse<ViajeResponse> newHttpErrorResponse(Error error)
        {
            return new HttpResponse<ViajeResponse>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ApiResponse = new ApiResponse<ViajeResponse>()
                {
                    Data = null,
                    Error = error
                }
            };
        }

        private bool esValido(ViajeRequest viaje, GeviApiContext ctx, Empleado emp)
        {
            var res = ctx.Viajes
                        .Where(v => v.Empleado.Email.Equals(emp.Email))
                        .ToList();
            bool valor = true;

            if (res.Count != 0)
            {
                foreach(var v in res)
                {
                    var inicioViajeNuevoValido = NotBetween(viaje.FechaInicio, v.FechaInicio, v.FechaFin);
                    var finViajeNuevoValido = NotBetween(viaje.FechaFin, v.FechaInicio, v.FechaFin);

                    if (inicioViajeNuevoValido && finViajeNuevoValido)
                    {
                        var inicioViajeExistenteValido = NotBetween(v.FechaInicio, viaje.FechaInicio, viaje.FechaFin);
                        var finViajeExistenteValido = NotBetween(v.FechaFin, viaje.FechaInicio, viaje.FechaFin);

                        return (inicioViajeExistenteValido && finViajeExistenteValido);
                    }

                    return false;
                }
            }

            return valor;
        }

        private bool NotBetween(DateTime input, DateTime date1, DateTime date2)
        {
            return (input < date1 || input > date2);
        }
    }
}