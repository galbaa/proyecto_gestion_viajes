using System.Linq;
using System.Data.Entity;
using Gevi.Api.Middleware.Interfaces;
using Gevi.Api.Models;
using Gevi.Api.Models.Responses;
using Nancy;
using System.Collections.Generic;

namespace Gevi.Api.Middleware
{
    public class LoginManager : ILoginManager
    {
        public HttpResponse<LoginResponse> Login(string username, string password)
        {
            var encryptionManager = new EncryptionManager();
            var pass = encryptionManager.Encryptdata(password);

            using (var db = new GeviApiContext())
            {
                var user = db.Usuarios.Where(u => u.Email.Equals(username)
                                && u.Contrasenia.Equals(pass)).FirstOrDefault();

                if(user != null)
                {
                    var clientes = db.Clientes
                                    .Include(c => c.Proyectos)
                                    .Include(c => c.Tipo)
                                    .ToList();

                    var tipoClientes = db.TipoClientes.ToList();
                    var monedas = db.Monedas.ToList();
                    var tipoGastos = db.TipoGastos.ToList();

                    var clientesResponse = new List<ClienteResponse>();
                    var tipoClientesResponse = new List<TipoResponse>();
                    var tipoGastosResponse = new List<TipoResponse>();

                    foreach (var c in clientes)
                    {
                        var nuevoCliente = new ClienteResponse()
                        {
                            Id = c.Id,
                            Nombre = c.Nombre,
                            Proyectos = null, // lo devuelvo null porque no me interesa devolver los proyectos
                            Tipo = c.Tipo
                        };

                        clientesResponse.Add(nuevoCliente);
                    }

                    foreach (var tc in tipoClientes)
                    {
                        var nuevoTipoCliente = new TipoResponse()
                        {
                            TipoNombre = tc.Nombre
                        };

                        tipoClientesResponse.Add(nuevoTipoCliente);
                    }

                    foreach (var tg in tipoGastos)
                    {
                        var nuevoTipoGasto = new TipoResponse()
                        {
                            TipoNombre = tg.Nombre
                        };

                        tipoGastosResponse.Add(nuevoTipoGasto);
                    }

                    return newHttpResponse(new LoginResponse()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FechaRegistro = user.FechaRegistro,
                        Nombre = user.Nombre,
                        Clientes = clientesResponse,
                        Monedas = monedas,
                        TipoClientes = tipoClientesResponse,
                        TipoGastos = tipoGastosResponse,
                        EsEmpleado = user is Empleado
                    });
                }
                else
                {
                    return newHttpErrorResponse(new Error("Login invalido. Verifique las credenciales."));
                }
            }
        }

        private HttpResponse<LoginResponse> newHttpResponse(LoginResponse response)
        {
            return new HttpResponse<LoginResponse>()
            {
                StatusCode = HttpStatusCode.OK,
                ApiResponse = new ApiResponse<LoginResponse>()
                {
                    Data = response,
                    Error = null
                }
            };
        }

        private HttpResponse<LoginResponse> newHttpErrorResponse(Error error)
        {
            return new HttpResponse<LoginResponse>()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                ApiResponse = new ApiResponse<LoginResponse>()
                {
                    Data = null,
                    Error = error
                }
            };
        }
    }
}