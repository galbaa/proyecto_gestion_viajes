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
    public class ProyectosManager : IProyectosManager
    {
        public HttpResponse<ProyectoResponse> NuevoProyecto(ProyectoRequest request)
        {
            if (request == null)
                return newHttpErrorResponse(new Error("El Proyecto que se intenta ingresar es invalido."));

            using (var db = new GeviApiContext())
            {
                var cli = db.Clientes
                                .Where(c => c.Nombre.Equals(request.ClienteNombre))
                                .Include(c => c.Proyectos)
                                .Include(c => c.Tipo)
                                .FirstOrDefault();

                if (cli != null)
                {
                    var nuevo = new Proyecto()
                    {
                        Nombre = request.Nombre,
                        Cliente = cli,
                        FechaInicio = request.FechaInicio
                    };

                    try
                    {
                        db.Proyectos.Add(nuevo);
                        db.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        return newHttpErrorResponse(new Error("Ya existe un Proyecto con ese nombre."));
                    }

                    var response = new ProyectoResponse()
                    {
                        Id = nuevo.Id,
                        Nombre = nuevo.Nombre,
                        FechaInicio = nuevo.FechaInicio,
                        Cliente = nuevo.Cliente?.Nombre
                    };

                    return newHttpResponse(response);
                }

                return newHttpErrorResponse(new Error("No existe el cliente."));
            }
        }
        public HttpResponse<ProyectoResponse> BorrarProyecto(ProyectoRequest request)
        {
            if (request == null)
                return newHttpErrorResponse(new Error("El proyecto que se intenta borrar es invalido."));

            using (var db = new GeviApiContext())
            {
                var pro = db.Proyectos
                                .Where(p => p.Nombre.Equals(request.Nombre))
                                .Include(c => c.Cliente)
                                .FirstOrDefault();

                if (pro == null)
                    return newHttpErrorResponse(new Error("No existe el Proyecto"));

                var viajes = db.Viajes
                                .Where(v => v.Proyecto.Id == pro.Id)
                                .Include(v => v.Empleado)
                                .Include(v => v.Gastos)
                                .Include(v => v.Proyecto)
                                .ToList();

                var response = new ProyectoResponse()
                {
                    Id = pro.Id,
                    Nombre = pro.Nombre,
                    FechaInicio = pro.FechaInicio,
                    Cliente = pro.Cliente?.Nombre
                };

                if (viajes != null)
                {
                    foreach (var v in viajes)
                    {
                        var gastos = db.Gastos
                            .Where(g => g.Viaje.Id == v.Id)
                            .ToList();

                        if (gastos != null)
                        {
                            foreach (var g in gastos)
                            {
                                db.Gastos.Remove(g);
                                db.SaveChanges();
                            }
                        }
                        db.Viajes.Remove(v);
                        db.SaveChanges();
                    }
                }
                db.Proyectos.Remove(pro);
                db.SaveChanges();
                
                return newHttpResponse(response);
            }
        }
        public HttpResponse<ProyectoResponse> ModificarProyecto(ProyectoRequest request)
        {
            if (request == null)
                return newHttpErrorResponse(new Error("El Proyecto que se intenta modificar es invalido."));

            using (var db = new GeviApiContext())
            {
                var pro = db.Proyectos
                                .Where(p => p.Nombre.Equals(request.Nombre))
                                .Include(p => p.Cliente)
                                .FirstOrDefault();

                if (pro == null)
                    return newHttpErrorResponse(new Error("No existe el Proyecto"));

                if (pro.Cliente != null &&
                    pro.Cliente.Nombre.Equals(request.ClienteNombre) &&
                    pro.Nombre.Equals(request.Nombre) &&
                    pro.FechaInicio == request.FechaInicio)
                {
                    return newHttpErrorResponse(new Error("El proyecto no se modifico"));
                }

                var cli = db.Clientes
                                .Where(c => c.Nombre.Equals(request.ClienteNombre))
                                .Include(c => c.Proyectos)
                                .Include(c => c.Tipo)
                                .FirstOrDefault();
                if (cli != null)
                {
                    pro.Nombre = request.Nombre;
                    pro.FechaInicio = request.FechaInicio;
                    pro.Cliente = cli;

                    db.Entry(pro).State = EntityState.Modified;
                    db.SaveChanges();

                    var response = new ProyectoResponse()
                    {
                        Id = pro.Id,
                        Nombre = pro.Nombre,
                        FechaInicio = pro.FechaInicio,
                        Cliente = pro.Cliente?.Nombre
                    };

                    return newHttpResponse(response);
                }

                return newHttpErrorResponse(new Error("No existe el cliente"));
            }
        }
        public HttpResponse<List<ProyectoResponse>> Todos()
        {
            using (var db = new GeviApiContext())
            {
                var proyectos = db.Proyectos
                                    .Include(p => p.Cliente)
                                    .ToList();

                var response = new List<ProyectoResponse>();

                foreach (var p in proyectos)
                {
                    var nuevo = new ProyectoResponse()
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        FechaInicio = p.FechaInicio,
                        Cliente = p.Cliente?.Nombre
                    };

                    response.Add(nuevo);
                }

                return newHttpListResponse(response);
            }
        }

        private HttpResponse<ProyectoResponse> newHttpResponse(ProyectoResponse response)
        {
            return new HttpResponse<ProyectoResponse>()
            {
                StatusCode = HttpStatusCode.OK,
                ApiResponse = new ApiResponse<ProyectoResponse>()
                {
                    Data = response,
                    Error = null
                }
            };
        }

        private HttpResponse<List<ProyectoResponse>> newHttpListResponse(List<ProyectoResponse> response)
        {
            return new HttpResponse<List<ProyectoResponse>>()
            {
                StatusCode = HttpStatusCode.OK,
                ApiResponse = new ApiResponse<List<ProyectoResponse>>()
                {
                    Data = response,
                    Error = null
                }
            };
        }

        private HttpResponse<ProyectoResponse> newHttpErrorResponse(Error error)
        {
            return new HttpResponse<ProyectoResponse>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ApiResponse = new ApiResponse<ProyectoResponse>()
                {
                    Data = null,
                    Error = error
                }
            };
        }
    }
}