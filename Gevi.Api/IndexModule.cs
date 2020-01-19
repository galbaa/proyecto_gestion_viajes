using Nancy;
using Gevi.Api.Models;
using Nancy.ModelBinding;
using Gevi.Api.Middleware.Interfaces;
using Gevi.Api.Models.Requests;

namespace Gevi.Api
{
    public class IndexModule : NancyModule
    {
        public IndexModule(ILoginManager loginManager,
            IUsuariosManager usuariosManager,
            IViajesManager viajesManager,
            IClientesManager clientesManager)
        {
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
                var clienteRequest = this.Bind<ClienteRequest>(cr => cr.Nombre);
                var clienteResponse = clientesManager.BorrarCliente(clienteRequest);

                return Negotiate
                    .WithContentType("application/json")
                    .WithStatusCode(clienteResponse.StatusCode)
                    .WithModel(clienteResponse.ApiResponse);
            };
        }
    }
}