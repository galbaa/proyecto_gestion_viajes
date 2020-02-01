﻿using Nancy;
using Gevi.Api.Models;
using Nancy.ModelBinding;
using Gevi.Api.Middleware.Interfaces;
using Gevi.Api.Models.Requests;
using Nancy.Authentication.Token;
using Nancy.Security;
using Nancy.Responses;

namespace Gevi.Api
{
    public class IndexModule : NancyModule
    {
        public IndexModule(IAccessAuthorizer accessAuthorizer,
            ILoginManager loginManager,
            IUsuariosManager usuariosManager,
            IViajesManager viajesManager,
            IClientesManager clientesManager,
            IProyectosManager proyectosManager)
        {
            Before += accessAuthorizer.Authorized;
            
            Post["login/standard"] = parameters =>
            {
                var loginRequest = this.Bind<LoginRequest>();
                var loginResponse = loginManager.Login(loginRequest.Username, loginRequest.Password);

                return Negotiate
                    .WithContentType("application/json")
                    .WithStatusCode(loginResponse.StatusCode)
                    .WithModel(loginResponse.ApiResponse);
            };

            Post["usuarios/nuevo"] = parameters =>
            {
                var usuarioRequest = this.Bind<UsuarioRequest>();
                var usuarioResponse = usuariosManager.NuevoUsuario(usuarioRequest);

                return Negotiate
                    .WithContentType("application/json")
                    .WithStatusCode(usuarioResponse.StatusCode)
                    .WithModel(usuarioResponse.ApiResponse);
            };

            Post["viajes/nuevo"] = parameters =>
            {
                var viajeRequest = this.Bind<ViajeRequest>();
                var viajeResponse = viajesManager.NuevoViaje(viajeRequest);

                return Negotiate
                    .WithContentType("application/json")
                    .WithStatusCode(viajeResponse.StatusCode)
                    .WithModel(viajeResponse.ApiResponse);
            };

            Put["viajes/validar"] = parameters =>
            {
                var validacionRequest = this.Bind<ValidacionRequest>();
                var viajeResponse = viajesManager.ValidarViaje(validacionRequest);

                return Negotiate
                    .WithContentType("application/json")
                    .WithStatusCode(viajeResponse.StatusCode)
                    .WithModel(viajeResponse.ApiResponse);
            };

            Post["viajes/historial"] = parameters =>
            {
                var viajeRequest = this.Bind<ViajeRequest>("estado", "fechaInicio", "fechaFin", "gastos", "proyecto");
                var viajeResponse = viajesManager.Historial(viajeRequest);

                return Negotiate
                    .WithContentType("application/json")
                    .WithStatusCode(viajeResponse.StatusCode)
                    .WithModel(viajeResponse.ApiResponse);
            };

            Get["viajes/todos"] = parameters =>
            {
                var viajeResponse = viajesManager.Todos();

                return Negotiate
                    .WithContentType("application/json")
                    .WithStatusCode(viajeResponse.StatusCode)
                    .WithModel(viajeResponse.ApiResponse);
            };

            Post["clientes/nuevo"] = parameters =>
            {
                var clienteRequest = this.Bind<ClienteRequest>();
                var clienteResponse = clientesManager.NuevoCliente(clienteRequest);

                return Negotiate
                    .WithContentType("application/json")
                    .WithStatusCode(clienteResponse.StatusCode)
                    .WithModel(clienteResponse.ApiResponse);
            };

            Delete["clientes/eliminar"] = parameters =>
            {
                var clienteRequest = this.Bind<ClienteRequest>("pais", "tipo");
                var clienteResponse = clientesManager.BorrarCliente(clienteRequest);

                return Negotiate
                    .WithContentType("application/json")
                    .WithStatusCode(clienteResponse.StatusCode)
                    .WithModel(clienteResponse.ApiResponse);
            };

            Put["clientes/modificar"] = parameters =>
            {
                var clienteRequest = this.Bind<ClienteRequest>();
                var clienteResponse = clientesManager.ModificarCliente(clienteRequest);

                return Negotiate
                    .WithContentType("application/json")
                    .WithStatusCode(clienteResponse.StatusCode)
                    .WithModel(clienteResponse.ApiResponse);
            };

            Get["clientes/todos"] = parameters =>
            {
                var clienteResponse = clientesManager.Todos();

                return Negotiate
                    .WithContentType("application/json")
                    .WithStatusCode(clienteResponse.StatusCode)
                    .WithModel(clienteResponse.ApiResponse);
            };

            Post["proyectos/nuevo"] = parameters =>
            {
                var proyectoRequest = this.Bind<ProyectoRequest>();
                var proyectoResponse = proyectosManager.NuevoProyecto(proyectoRequest);

                return Negotiate
                    .WithContentType("application/json")
                    .WithStatusCode(proyectoResponse.StatusCode)
                    .WithModel(proyectoResponse.ApiResponse);
            };

            Delete["proyectos/eliminar"] = parameters =>
            {
                var proyectoRequest = this.Bind<ProyectoRequest>("clienteNombre");
                var proyectoResponse = proyectosManager.BorrarProyecto(proyectoRequest);

                return Negotiate
                    .WithContentType("application/json")
                    .WithStatusCode(proyectoResponse.StatusCode)
                    .WithModel(proyectoResponse.ApiResponse);
            };

            Put["proyectos/modificar"] = parameters =>
            {
                var proyectoRequest = this.Bind<ProyectoRequest>();
                var proyectoResponse = proyectosManager.ModificarProyecto(proyectoRequest);

                return Negotiate
                    .WithContentType("application/json")
                    .WithStatusCode(proyectoResponse.StatusCode)
                    .WithModel(proyectoResponse.ApiResponse);
            };

            Get["proyectos/todos"] = parameters =>
            {
                var proyectoResponse = proyectosManager.Todos();

                return Negotiate
                    .WithContentType("application/json")
                    .WithStatusCode(proyectoResponse.StatusCode)
                    .WithModel(proyectoResponse.ApiResponse);
            };
        }
    }
}