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
    public class ViajesManager : IViajesManager
    {
        public HttpResponse<ViajeResponse> NuevoViaje(ViajeRequest request)
        {
            if (request == null)
                return newHttpErrorResponse(new Error("El viaje que se intenta ingresar es invalido."));

            using (var db = new GeviApiContext())
            {
                var empleado = db.Usuarios
                                    .OfType<Empleado>()
                                    .Where(u => u is Empleado && u.Id == request.EmpleadoId)
                                    .Include(u => u.Viajes)
                                    .FirstOrDefault();

                var proyecto = db.Proyectos
                                    .Where(p => p.Nombre.Equals(request.ProyectoNombre))
                                    .Include(p => p.Cliente)
                                    .FirstOrDefault();

                if (empleado != null && proyecto != null && esValido(request, db, empleado) )
                {
                    var nuevo = new Viaje()
                    {
                        Empleado = empleado,
                        Estado = Estado.PENDIENTE_APROBACION,
                        FechaInicio = request.FechaInicio,
                        FechaFin = request.FechaFin,
                        Gastos = null,
                        Proyecto = proyecto
                    };

                    try
                    {
                        db.Viajes.Add(nuevo);
                        db.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        return newHttpErrorResponse(new Error("Error al ingresar el viaje."));
                    }

                    var response = new ViajeResponse()
                    {
                        Id = nuevo.Id,
                        EmpleadoId = empleado.Id,
                        Estado = Estado.PENDIENTE_APROBACION,
                        FechaInicio = request.FechaInicio,
                        FechaFin = request.FechaFin,
                        Gastos = null,
                        Proyecto = proyecto.Nombre
                    };

                    return newHttpResponse(response);
                }
                else
                {
                    if (empleado == null)
                        return newHttpErrorResponse(new Error("No existe el empleado"));
                    else
                    {
                        if (proyecto == null)
                            return newHttpErrorResponse(new Error("No existe el proyecto"));
                        else
                            return newHttpErrorResponse(new Error("Ya existe un viaje en esa fecha para el empleado."));
                    }
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
                    .Where(v => v.Id == request.Id)
                    .Include(u => u.Empleado)
                    .Include(w => w.Proyecto.Cliente)
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
                        EmpleadoNombre = viaje.Empleado?.Nombre,
                        Estado = request.Estado,
                        FechaInicio = viaje.FechaInicio,
                        FechaFin = viaje.FechaFin,
                        Gastos = null,
                        Proyecto = viaje.Proyecto?.Nombre,
                        ClienteProyectoNombre = viaje.Proyecto?.Cliente?.Nombre
                    };

                    if (viaje.Gastos != null)
                    {
                        var gastos = new List<GastoResponse>();

                        foreach (var g in viaje.Gastos)
                        {
                            var nuevoGasto = new GastoResponse()
                            {
                                Id = g.Id,
                                Estado = g.Estado,
                                Fecha = g.Fecha,
                                Moneda = g.Moneda?.Nombre,
                                Tipo = g.Tipo?.Nombre,
                                Total = g.Total,
                                ViajeId = viaje.Id,
                                Proyecto = g.Viaje?.Proyecto?.Nombre,
                                Empleado = g.Empleado?.Nombre
                            };

                            gastos.Add(nuevoGasto);
                        }

                        response.Gastos = gastos;
                    }

                    return newHttpResponse(response);
                }
                return newHttpErrorResponse(new Error("No existe el viaje"));
            }
        }

        public HttpResponse<List<ViajeResponse>> Historial(ViajeRequest request)
        {
            using (var db = new GeviApiContext())
            {
                var empleado = db.Usuarios
                                    .OfType<Empleado>()
                                    .Where(u => u is Empleado && u.Id == request.EmpleadoId)
                                    .Include(u => u.Viajes)
                                    .FirstOrDefault();

                if (empleado != null)
                {

                    var viajes = empleado?.Viajes;

                    var response = new List<ViajeResponse>();

                    if (viajes != null)
                    {
                        foreach (var v in viajes)
                        {
                            var nuevo = new ViajeResponse()
                            {
                                Id = v.Id,
                                EmpleadoId = request.EmpleadoId,
                                Estado = v.Estado,
                                FechaFin = v.FechaFin,
                                FechaInicio = v.FechaInicio,
                                Gastos = null,
                                Proyecto = v.Proyecto?.Nombre
                            };

                            if (v.Gastos != null)
                            {
                                var gastosRespone = new List<GastoResponse>();

                                foreach (var g in v.Gastos)
                                {
                                    var nuevoGastoResponse = new GastoResponse()
                                    {
                                        Id = g.Id,
                                        Estado = g.Estado,
                                        Fecha = g.Fecha,
                                        Moneda = g.Moneda?.Nombre,
                                        Tipo = g.Tipo?.Nombre,
                                        ViajeId = v.Id,
                                        Proyecto = g.Viaje?.Proyecto?.Nombre,
                                        Total = g.Total,
                                        Empleado = g.Empleado?.Nombre
                                    };

                                    gastosRespone.Add(nuevoGastoResponse);
                                }

                                nuevo.Gastos = gastosRespone;
                            }

                            response.Add(nuevo);
                        }
                    }
                    return newHttpListResponse(response);
                }

                return newHttpErrorListResponse(new Error("No existe el empleado"));
            }
        }

        public HttpResponse<List<ViajeResponse>> Pendientes()
        {
            using (var db = new GeviApiContext())
            {
                var viajes = db.Viajes
                                .Where(v => v.Estado == Estado.PENDIENTE_APROBACION)
                                .Include(v => v.Empleado)
                                .Include(v => v.Gastos)
                                .Include(v => v.Proyecto.Cliente)
                                .ToList();

                var response = new List<ViajeResponse>();

                foreach (var v in viajes)
                {
                    var nuevo = new ViajeResponse()
                    {
                        Id = v.Id,
                        EmpleadoId = v.Empleado.Id,
                        Estado = v.Estado,
                        FechaFin = v.FechaFin,
                        FechaInicio = v.FechaInicio,
                        Gastos = null,
                        Proyecto = v.Proyecto?.Nombre,
                        ClienteProyectoNombre = v.Proyecto?.Cliente?.Nombre,
                        EmpleadoNombre = v.Empleado?.Nombre
                    };

                    if (v.Gastos != null)
                    {
                        var gastosRespone = new List<GastoResponse>();

                        foreach (var g in v.Gastos)
                        {
                            var nuevoGastoResponse = new GastoResponse()
                            {
                                Id = g.Id,
                                Estado = g.Estado,
                                Fecha = g.Fecha,
                                Moneda = g.Moneda?.Nombre,
                                Tipo = g.Tipo?.Nombre,
                                ViajeId = v.Id,
                                Proyecto = g.Viaje?.Proyecto?.Nombre,
                                Total = g.Total,
                                Empleado = g.Empleado?.Nombre
                            };

                            gastosRespone.Add(nuevoGastoResponse);
                        }

                        nuevo.Gastos = gastosRespone;
                    }

                    response.Add(nuevo);
                }

                return newHttpListResponse(response);
            }
        }

        public HttpResponse<List<ViajeResponse>> Todos()
        {
            using (var db = new GeviApiContext())
            {
                var viajes = db.Viajes
                                .Include(v => v.Empleado)
                                .Include(v => v.Gastos)
                                .Include(v => v.Proyecto)
                                .ToList();

                var response = new List<ViajeResponse>();

                foreach (var v in viajes)
                {
                    var nuevo = new ViajeResponse()
                    {
                        Id = v.Id,
                        EmpleadoId = v.Empleado.Id,
                        Estado = v.Estado,
                        FechaFin = v.FechaFin,
                        FechaInicio = v.FechaInicio,
                        Gastos = null,
                        Proyecto = v.Proyecto?.Nombre
                    };

                    if (v.Gastos != null)
                    {
                        var gastosRespone = new List<GastoResponse>();

                        foreach (var g in v.Gastos)
                        {
                            var nuevoGastoResponse = new GastoResponse()
                            {
                                Id = g.Id,
                                Estado = g.Estado,
                                Fecha = g.Fecha,
                                Moneda = g.Moneda?.Nombre,
                                Tipo = g.Tipo?.Nombre,
                                ViajeId = v.Id,
                                Proyecto = g.Viaje?.Proyecto?.Nombre,
                                Total = g.Total,
                                Empleado = g.Empleado?.Nombre
                            };

                            gastosRespone.Add(nuevoGastoResponse);
                        }

                        nuevo.Gastos = gastosRespone;
                    }

                    response.Add(nuevo);
                }

                return newHttpListResponse(response);
            }
        }

        public HttpResponse<List<ViajeResponse>> EntreFechas(ListadoViajesRequest request)
        {
            using (var db = new GeviApiContext())
            {
                var viajes = db.Viajes
                                    .Where(v => (!request.FechaInicio.Equals(DateTime.MinValue) ? v.FechaInicio >= request.FechaInicio : true) &&
                                                (!request.FechaFin.Equals(DateTime.MinValue) ? v.FechaFin <= request.FechaFin : true) &&
                                                (!String.IsNullOrEmpty(request.ClienteNombre) && (v.Proyecto != null) && (v.Proyecto.Cliente != null) ? v.Proyecto.Cliente.Nombre.Equals(request.ClienteNombre) : true))
                                    .Include(v => v.Empleado)
                                    .Include(v => v.Gastos.Select(g => g.Tipo))
                                    .Include(v => v.Proyecto)
                                    .ToList();

                var response = new List<ViajeResponse>();

                foreach (var v in viajes)
                {
                    var nuevo = new ViajeResponse()
                    {
                        Id = v.Id,
                        EmpleadoId = v.Empleado.Id,
                        Estado = v.Estado,
                        FechaFin = v.FechaFin,
                        FechaInicio = v.FechaInicio,
                        Gastos = null,
                        Proyecto = v.Proyecto?.Nombre
                    };

                    if (v.Gastos != null)
                    {
                        var gastosRespone = new List<GastoResponse>();

                        foreach (var g in v.Gastos)
                        {
                            var nuevoGastoResponse = new GastoResponse()
                            {
                                Id = g.Id,
                                Estado = g.Estado,
                                Fecha = g.Fecha,
                                Moneda = g.Moneda?.Nombre,
                                Tipo = g.Tipo?.Nombre,
                                ViajeId = v.Id,
                                Proyecto = g.Viaje?.Proyecto?.Nombre,
                                Total = g.Total,
                                Empleado = g.Empleado?.Nombre
                            };

                            gastosRespone.Add(nuevoGastoResponse);
                        }

                        nuevo.Gastos = gastosRespone;
                    }

                    response.Add(nuevo);
                }

                return newHttpListResponse(response);
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

        private HttpResponse<List<ViajeResponse>> newHttpListResponse(List<ViajeResponse> response)
        {
            return new HttpResponse<List<ViajeResponse>>()
            {
                StatusCode = HttpStatusCode.OK,
                ApiResponse = new ApiResponse<List<ViajeResponse>>()
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

        private HttpResponse<List<ViajeResponse>> newHttpErrorListResponse(Error error)
        {
            return new HttpResponse<List<ViajeResponse>>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ApiResponse = new ApiResponse<List<ViajeResponse>>()
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

            bool esValido = true;

            if (res.Count != 0)
            {
                foreach(var v in res)
                {
                    if (esValido)
                    {
                        var inicioViajeNuevoValido = NotBetween(viaje.FechaInicio, v.FechaInicio, v.FechaFin);
                        var finViajeNuevoValido = NotBetween(viaje.FechaFin, v.FechaInicio, v.FechaFin);

                        if (inicioViajeNuevoValido && finViajeNuevoValido)
                        {
                            var inicioViajeExistenteValido = NotBetween(v.FechaInicio, viaje.FechaInicio, viaje.FechaFin);
                            var finViajeExistenteValido = NotBetween(v.FechaFin, viaje.FechaInicio, viaje.FechaFin);

                            esValido = inicioViajeExistenteValido && finViajeExistenteValido;
                        }
                        else
                            esValido = false;
                    }
                }
            }

            return esValido;
        }

        private bool NotBetween(DateTime input, DateTime date1, DateTime date2)
        {
            return (input < date1 || input > date2);
        }
    }
}