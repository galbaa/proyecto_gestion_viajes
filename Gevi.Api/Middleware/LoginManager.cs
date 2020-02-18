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
                var user = db.Usuarios
                                .Where(u => u.Email.Equals(username) && u.Contrasenia.Equals(pass))
                                .FirstOrDefault();

                if (user != null)
                {
                    #region cargarListas
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
                    #endregion

                    if (user is Empleado)
                    {
                        Empleado usuario = user as Empleado;

                        var viajes = db.Viajes
                                     .Where(v => v.Empleado.Id == usuario.Id)
                                     .Where(v => v.Estado == Estado.APROBADO)
                                     .Include(v => v.Empleado)
                                     .Include(v => v.Gastos)
                                     .Include(v => v.Proyecto)
                                     .ToList();

                        var response = new List<ViajeResponse>();

                        #region traerViajesDelEmpleado
                        if (viajes != null)
                        {
                            foreach (var v in viajes)
                            {
                                var nuevo = new ViajeResponse()
                                {
                                    Id = v.Id,
                                    EmpleadoId = usuario.Id,
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
                        #endregion

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
                            EsEmpleado = user is Empleado,
                            ViajesAprobados = response
                        });
                    }

                    if (user is Administrativo)
                    {
                        var empleados = db.Usuarios
                                            .OfType<Empleado>()
                                            .ToList();

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
                            EsEmpleado = user is Empleado,
                            Empleados = empleados
                        });
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