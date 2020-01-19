using Gevi.Api.Middleware.Interfaces;
using Gevi.Api.Models;
using Gevi.Api.Models.Requests;
using Gevi.Api.Models.Responses;
using Nancy;
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

                var response = new ClienteResponse()
                {
                    Nombre = request.Nombre,
                    Pais = request.Pais,
                    Proyectos = null,
                    Tipo = tipoCli
                };

                var nuevo = new Cliente()
                {
                    Nombre = request.Nombre,
                    Pais = request.Pais,
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
                                .FirstOrDefault();

                if (cli == null)
                    return newHttpErrorResponse(new Error("No existe el cliente"));

                var proyectos = db.Proyectos
                                    .Where(p => p.Cliente.Id == cli.Id)
                                    .ToList();

                foreach (var p in proyectos)
                {
                    db.Proyectos.Remove(p);
                }

                var response = new ClienteResponse()
                {
                    Id = cli.Id,
                    Nombre = cli.Nombre,
                    Pais = cli.Pais,
                    Proyectos = cli.Proyectos,
                    Tipo = cli.Tipo
                };

                    db.Clientes.Remove(cli);
                    db.SaveChanges();
                
                return newHttpResponse(response);
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