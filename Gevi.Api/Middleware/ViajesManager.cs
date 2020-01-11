using Gevi.Api.Middleware.Interfaces;
using Gevi.Api.Models;
using Nancy;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Gevi.Api.Middleware
{
    public class ViajesManager : IViajesManager
    {
        public HttpResponse<Viaje> NuevoViaje(ViajeRequest viaje)
        {
            if (viaje == null)
                return newHttpErrorResponse(new Error("El viaje que se intenta ingresar es invalido."));

            using (var db = new GeviApiContext())
            {
                var empleado = (Empleado)db.Usuarios.Where(u => u is Empleado && u.Id == viaje.EmpleadoId).FirstOrDefault();

                if (esValido(viaje, db, empleado))
                {
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
                        db.SaveChangesAsync();
                    }
                    catch (DbUpdateException)
                    {
                        return newHttpErrorResponse(new Error("Ya existe un viaje con ese email."));
                    }

                    return newHttpResponse(nuevo);
                }
                else
                {
                    return newHttpErrorResponse(new Error("Ya existe un viaje en esa fecha para el empleado."));
                }
            }
        }

        private HttpResponse<Viaje> newHttpResponse(Viaje response)
        {
            return new HttpResponse<Viaje>()
            {
                StatusCode = HttpStatusCode.OK,
                ApiResponse = new ApiResponse<Viaje>()
                {
                    Data = response,
                    Error = null
                }
            };
        }

        private HttpResponse<Viaje> newHttpErrorResponse(Error error)
        {
            return new HttpResponse<Viaje>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ApiResponse = new ApiResponse<Viaje>()
                {
                    Data = null,
                    Error = error
                }
            };
        }

        private bool esValido(ViajeRequest viaje, GeviApiContext ctx, Empleado emp)
        {
            var res = ctx.Viajes.Where(v => v.Empleado.Email.Equals(emp.Email)).FirstOrDefault();

            var valorFechaInicio = DateTime.Compare(viaje.FechaInicio, res.FechaInicio);
            var valorFechaFin = DateTime.Compare(viaje.FechaFin, res.FechaFin);

            bool valor = valorFechaInicio > 0 && valorFechaFin < 0;

            return valor;
        }
    }
}