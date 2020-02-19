using System;
using System.Collections.Generic;
using Gevi.Api.Middleware.Interfaces;
using Gevi.Api.Models;
using Gevi.Api.Models.Responses;
using Moq;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json;
using Xunit;

namespace Gevi.Api.Tests
{
    public class IndexModuleTest
    {
        private readonly Browser browser;
        private Action<BrowserContext> browserContext;

        private readonly Mock<IAccessAuthorizer> accessAuthorizer;
        private readonly Mock<ILoginManager> loginManager;
        private readonly Mock<IHeaderParser> headerParser;
        private readonly Mock<IUsuariosManager> usuariosManager;
        private readonly Mock<IViajesManager> viajesManager;
        private readonly Mock<IClientesManager> clientesManager;
        private readonly Mock<IProyectosManager> proyectosManager;
        private readonly Mock<IGastosManager> gastosManager;

        public IndexModuleTest()
        {
            accessAuthorizer = new Mock<IAccessAuthorizer>();
            loginManager = new Mock<ILoginManager>();
            headerParser = new Mock<IHeaderParser>();
            usuariosManager = new Mock<IUsuariosManager>();
            viajesManager = new Mock<IViajesManager>();
            clientesManager = new Mock<IClientesManager>();
            proyectosManager = new Mock<IProyectosManager>();
            gastosManager = new Mock<IGastosManager>();

            browser = new Browser(config =>
            {
                config.Dependency(accessAuthorizer.Object);
                config.Dependency(loginManager.Object);
                config.Dependency(usuariosManager.Object);
                config.Dependency(viajesManager.Object);
                config.Dependency(clientesManager.Object);
                config.Dependency(proyectosManager.Object);
                config.Dependency(gastosManager.Object);
            });

            headerParser
                .Setup(hp => hp.GetApiKey(It.IsAny<NancyContext>()))
                .Returns("some-api-key");
        }


        [Fact]
        public void login_standard_call()
        {
            var browserResponse = CallEndpoint("/login/standard", "POST", "{'username' : 'some-username', 'password' : 'some-password'}");
            var response = JsonConvert.DeserializeObject<ApiResponse<LoginResponse>>(browserResponse.Body.AsString());
        }

        [Fact]
        public void usuarios_nuevo_call()
        {
            var browserResponse = CallEndpoint("/usuarios/nuevo", "POST");
            var response = JsonConvert.DeserializeObject<ApiResponse<UsuarioResponse>>(browserResponse.Body.AsString());
        }

        [Fact]
        public void usuarios_modificar_call()
        {
            var browserResponse = CallEndpoint("/usuarios/modificar", "PUT");
            var response = JsonConvert.DeserializeObject<ApiResponse<UsuarioResponse>>(browserResponse.Body.AsString());
        }

        [Fact]
        public void viajes_nuevo_call()
        {
            var browserResponse = CallEndpoint("/viajes/nuevo", "POST");
            var response = JsonConvert.DeserializeObject<ApiResponse<ViajeResponse>>(browserResponse.Body.AsString());
        }

        [Fact]
        public void viajes_validar_call()
        {
            var browserResponse = CallEndpoint("/viajes/validar", "PUT");
            var response = JsonConvert.DeserializeObject<ApiResponse<ViajeResponse>>(browserResponse.Body.AsString());
        }
        [Fact]
        public void viajes_historial_call()
        {
            var browserResponse = CallEndpoint("/viajes/historial", "POST");
            var response = JsonConvert.DeserializeObject<ApiResponse<List<ViajeResponse>>>(browserResponse.Body.AsString());
        }
        [Fact]
        public void viajes_entrefechas_call()
        {
            var browserResponse = CallEndpoint("/viajes/entrefechas", "POST");
            var response = JsonConvert.DeserializeObject<ApiResponse<List<ViajeResponse>>>(browserResponse.Body.AsString());
        }

        [Fact]
        public void viajes_todos_call()
        {
            var browserResponse = CallEndpoint("/viajes/todos", "GET");
            var response = JsonConvert.DeserializeObject<ApiResponse<List<ViajeResponse>>>(browserResponse.Body.AsString());
        }

        [Fact]
        public void viajes_pendientes_call()
        {
            var browserResponse = CallEndpoint("/viajes/pendientes", "GET");
            var response = JsonConvert.DeserializeObject<ApiResponse<List<ViajeResponse>>>(browserResponse.Body.AsString());
        }

        [Fact]
        public void clientes_nuevo_call()
        {
            var browserResponse = CallEndpoint("/clientes/nuevo", "POST");
            var response = JsonConvert.DeserializeObject<ApiResponse<ClienteResponse>>(browserResponse.Body.AsString());
        }

        [Fact]
        public void clientes_eliminar_call()
        {
            var browserResponse = CallEndpoint("/clientes/eliminar", "DELETE");
            var response = JsonConvert.DeserializeObject<ApiResponse<ClienteResponse>>(browserResponse.Body.AsString());
        }

        [Fact]
        public void clientes_modificar_call()
        {
            var browserResponse = CallEndpoint("/clientes/modificar", "PUT");
            var response = JsonConvert.DeserializeObject<ApiResponse<ClienteResponse>>(browserResponse.Body.AsString());
        }

        [Fact]
        public void clientes_todos_call()
        {
            var browserResponse = CallEndpoint("/clientes/todos", "GET");
            var response = JsonConvert.DeserializeObject<ApiResponse<List<ClienteResponse>>>(browserResponse.Body.AsString());
        }

        [Fact]
        public void proyectos_nuevo_call()
        {
            var browserResponse = CallEndpoint("/proyectos/nuevo", "POST");
            var response = JsonConvert.DeserializeObject<ApiResponse<ProyectoResponse>>(browserResponse.Body.AsString());
        }

        [Fact]
        public void proyectos_eliminar_call()
        {
            var browserResponse = CallEndpoint("/proyectos/eliminar", "DELETE");
            var response = JsonConvert.DeserializeObject<ApiResponse<ProyectoResponse>>(browserResponse.Body.AsString());
        }

        [Fact]
        public void proyectos_modificar_call()
        {
            var browserResponse = CallEndpoint("/proyectos/modificar", "PUT");
            var response = JsonConvert.DeserializeObject<ApiResponse<ProyectoResponse>>(browserResponse.Body.AsString());
        }

        [Fact]
        public void proyectos_todos_call()
        {
            var browserResponse = CallEndpoint("/proyectos/todos", "GET");
            var response = JsonConvert.DeserializeObject<ApiResponse<List<ProyectoResponse>>>(browserResponse.Body.AsString());
        }

        [Fact]
        public void gastos_nuevo_call()
        {
            var browserResponse = CallEndpoint("/gastos/nuevo", "POST");
            var response = JsonConvert.DeserializeObject<ApiResponse<GastoResponse>>(browserResponse.Body.AsString());
        }

        [Fact]
        public void gastos_validar_call()
        {
            var browserResponse = CallEndpoint("/gastos/validar", "PUT");
            var response = JsonConvert.DeserializeObject<ApiResponse<GastoResponse>>(browserResponse.Body.AsString());
        }

        [Fact]
        public void gastos_pendientes_call()
        {
            var browserResponse = CallEndpoint("/gastos/pendientes", "GET");
            var response = JsonConvert.DeserializeObject<ApiResponse<List<GastoResponse>>>(browserResponse.Body.AsString());
        }

        [Fact]
        public void gastos_estadisticas_call()
        {
            var browserResponse = CallEndpoint("/gastos/estadisticas", "POST");
            var response = JsonConvert.DeserializeObject<ApiResponse<List<GastoResponse>>>(browserResponse.Body.AsString());
        }

        [Fact]
        public void usuarios_cambiarcontrasenia_call()
        {
            var browserResponse = CallEndpoint("/usuarios/cambiarcontrasenia", "PUT");
            var response = JsonConvert.DeserializeObject<ApiResponse<UsuarioResponse>>(browserResponse.Body.AsString());
        }

        private BrowserResponse CallEndpoint(string endpoint, string method, string body = "")
        {
            browserContext = with =>
            {
                with.HttpRequest();
                with.Header("Content-Type", "application/json");
                with.Header("Accept", "application/json");
                with.Body(body);
            };

            return  method.Equals("GET")    ? browser.Get(endpoint, browserContext) :
                    method.Equals("POST")   ? browser.Post(endpoint, browserContext) :
                    method.Equals("PUT")    ? browser.Put(endpoint, browserContext) :
                                              browser.Delete(endpoint, browserContext);
        }
    }
}
