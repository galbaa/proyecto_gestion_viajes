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
    public class ClientesManager : IClientesManager
    {
        public HttpResponse<ClienteResponse> NuevoCliente(ClienteRequest request)
        {
            if (request == null)
                return newHttpErrorResponse(new Error("El cliente que se intenta ingresar es invalido."));

            using (var db = new GeviApiContext())
            {
                var tipoCli = db.TipoClientes
                                .Where(tc => tc.Nombre == request.Tipo)
                                .FirstOrDefault();

                var nuevo = new Cliente()
                {
                    Nombre = request.Nombre,
                    Proyectos = null,
                    Tipo = tipoCli
                };

                try
                {
                    db.Clientes.Add(nuevo);
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    return newHttpErrorResponse(new Error("Ya existe un cliente con ese nombre."));
                }

                var response = new ClienteResponse()
                {
                    Id = nuevo.Id,
                    Nombre = nuevo.Nombre,
                    Proyectos = null,
                    Tipo = tipoCli
                };

                return newHttpResponse(response);
            }
        }

        public HttpResponse<ClienteResponse> BorrarCliente(ClienteRequest request)
        {
            if (request == null)
                return newHttpErrorResponse(new Error("El cliente que se intenta borrar es invalido."));

            using (var db = new GeviApiContext())
            {
                var cli = db.Clientes
                                .Where(c => c.Nombre == request.Nombre)
                                .Include(c => c.Proyectos.Select(p => p.Cliente))
                                .Include(c => c.Tipo)
                                .FirstOrDefault();

                if (cli == null)
                    return newHttpErrorResponse(new Error("No existe el cliente"));

                var proyectos = db.Proyectos
                                    .Where(p => p.Cliente.Id == cli.Id)
                                    .Include(p => p.Cliente)
                                    .ToList();

                var response = new ClienteResponse()
                {
                    Id = cli.Id,
                    Nombre = cli.Nombre,
                    Proyectos = null,
                    Tipo = cli.Tipo
                };

                if (proyectos != null)
                {
                    var proyectosResponse = new List<ProyectoResponse>();

                    foreach (var p in proyectos)
                    {
                        var nuevoProyResponse = new ProyectoResponse()
                        {
                            Id = p.Id,
                            Nombre = p.Nombre,
                            Cliente = cli?.Nombre
                        };
                        proyectosResponse.Add(nuevoProyResponse);

                        var viajes = db.Viajes
                            .Where(v => v.Proyecto.Id == p.Id)
                            .Include(v => v.Proyecto)
                            .ToList();

                        if (viajes != null)
                        {
                            foreach (var v in viajes)
                            {
                                db.Viajes.Remove(v);
                                db.SaveChanges();
                            }
                        }
                        db.Proyectos.Remove(p);
                        db.SaveChanges();
                    }

                    response.Proyectos = proyectosResponse;
                }

                db.Clientes.Remove(cli);
                db.SaveChanges();
                
                return newHttpResponse(response);
            }
        }
        public HttpResponse<ClienteResponse> ModificarCliente(ClienteRequest request)
        {
            if (request == null)
                return newHttpErrorResponse(new Error("El cliente que se intenta modificar es invalido."));

            using (var db = new GeviApiContext())
            {
                var cli = db.Clientes
                                .Where(c => c.Nombre.Equals(request.Nombre))
                                .Include(c => c.Proyectos)
                                .FirstOrDefault();

                var tipo = db.TipoClientes
                                .Where(t => t.Nombre.Equals(request.Tipo))
                                .FirstOrDefault();

                if (cli == null)
                    return newHttpErrorResponse(new Error("No existe el cliente"));

                cli.Nombre = request.Nombre;
                cli.Tipo = tipo;

                db.Entry(cli).State = EntityState.Modified;
                db.SaveChanges();

                var response = new ClienteResponse()
                {
                    Id = cli.Id,
                    Nombre = cli.Nombre,
                    Tipo = cli.Tipo,
                    Proyectos = null
                };

                if (cli.Proyectos != null)
                {
                    var proyectosResponse = new List<ProyectoResponse>();

                    foreach (var p in cli.Proyectos)
                    {
                        var nuevoProyResponse = new ProyectoResponse()
                        {
                            Id = p.Id,
                            Nombre = p.Nombre,
                            Cliente = cli.Nombre
                        };
                        proyectosResponse.Add(nuevoProyResponse);
                    }
                    response.Proyectos = proyectosResponse;
                }

                return newHttpResponse(response);
            }

        }

        public HttpResponse<ClienteResponse> BuscarCliente(ClienteRequest request)
        {
            if (request == null)
                return newHttpErrorResponse(new Error("El cliente que se intenta buscar es invalido."));

            using (var db = new GeviApiContext())
            {
                var cli = db.Clientes
                                .Where(c => c.Nombre.Equals(request.Nombre))
                                .Include(c => c.Proyectos)
                                .FirstOrDefault();

                if (cli == null)
                    return newHttpErrorResponse(new Error("No existe el cliente"));

                var response = new ClienteResponse()
                {
                    Id = cli.Id,
                    Nombre = cli.Nombre,
                    Proyectos = null,
                    Tipo = cli.Tipo
                };

                if (cli.Proyectos != null)
                {
                    var proyectosResponse = new List<ProyectoResponse>();

                    foreach (var p in cli.Proyectos)
                    {
                        var nuevoProyResponse = new ProyectoResponse()
                        {
                            Id = p.Id,
                            Nombre = p.Nombre,
                            Cliente = cli.Nombre
                        };
                        proyectosResponse.Add(nuevoProyResponse);
                    }
                    response.Proyectos = proyectosResponse;
                }

                return newHttpResponse(response);
            }
        }

        public HttpResponse<List<ClienteResponse>> Todos()
        {
            using (var db = new GeviApiContext())
            {
                var clientes = db.Clientes
                                    .Include(c => c.Proyectos)
                                    .Include(c => c.Tipo)
                                    .ToList();

                var response = new List<ClienteResponse>();

                foreach (var c in clientes)
                {
                    var nuevo = new ClienteResponse()
                    {
                        Id = c.Id,
                        Nombre = c.Nombre,
                        Tipo = c.Tipo,
                        Proyectos = null
                    };

                    if (c.Proyectos != null)
                    {
                        var proyectoRespone = new List<ProyectoResponse>();

                        foreach (var p in c.Proyectos)
                        {
                            var nuevoProyectoResponse = new ProyectoResponse()
                            {
                                Id = p.Id,
                                Nombre = p.Nombre,
                                Cliente = p.Cliente?.Nombre
                            };

                            proyectoRespone.Add(nuevoProyectoResponse);
                        }

                        nuevo.Proyectos = proyectoRespone;
                    }

                    response.Add(nuevo);
                }

                return newHttpListResponse(response);
            }
        }

        private HttpResponse<ClienteResponse> newHttpResponse(ClienteResponse response)
        {
            return new HttpResponse<ClienteResponse>()
            {
                StatusCode = HttpStatusCode.OK,
                ApiResponse = new ApiResponse<ClienteResponse>()
                {
                    Data = response,
                    Error = null
                }
            };
        }

        private HttpResponse<List<ClienteResponse>> newHttpListResponse(List<ClienteResponse> response)
        {
            return new HttpResponse<List<ClienteResponse>>()
            {
                StatusCode = HttpStatusCode.OK,
                ApiResponse = new ApiResponse<List<ClienteResponse>>()
                {
                    Data = response,
                    Error = null
                }
            };
        }

        private HttpResponse<ClienteResponse> newHttpErrorResponse(Error error)
        {
            return new HttpResponse<ClienteResponse>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ApiResponse = new ApiResponse<ClienteResponse>()
                {
                    Data = null,
                    Error = error
                }
            };
        }
    }
}