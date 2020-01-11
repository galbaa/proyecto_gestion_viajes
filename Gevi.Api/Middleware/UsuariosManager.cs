using Gevi.Api.Middleware.Interfaces;
using Gevi.Api.Models;
using Nancy;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Gevi.Api.Middleware
{
    public class UsuariosManager : IUsuariosManager
    {
        public HttpResponse<UsuarioResponse> NuevoUsuario(UsuarioRequest usuario)
        {
            if (usuario == null)
                return newHttpErrorResponse(new Error("El usuario que se intenta ingresar es invalido."));

            Usuario nuevo = null;

            switch (usuario.EsEmpleado)
            {
                case true:
                    nuevo = new Empleado()
                    {
                        Email = usuario.Email,
                        Contrasenia = usuario.Contrasenia,
                        Nombre = usuario.Nombre,
                        FechaRegistro = DateTime.Now,
                        Viajes = null
                    };
                    break;
                case false:
                    nuevo = new Administrativo()
                    {
                        Email = usuario.Email,
                        Contrasenia = usuario.Contrasenia,
                        Nombre = usuario.Nombre,
                        FechaRegistro = DateTime.Now
                    };
                    break;
                default:
                    break;
            }
            var encryptionManager = new EncryptionManager();
            nuevo.Contrasenia = encryptionManager.Encryptdata(nuevo.Contrasenia);

            using (var db = new GeviApiContext())
            {
                try
                {
                    db.Usuarios.Add(nuevo);
                    db.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    return newHttpErrorResponse(new Error("Ya existe un usuario con ese email."));
                }

                return newHttpResponse(new UsuarioResponse()
                {
                    Id = db.Usuarios.OrderByDescending(u => u.Id).Select(u => u.Id).FirstOrDefault(),
                    Email = nuevo.Email,
                    Nombre = nuevo.Nombre,
                    FechaRegistro = DateTime.Today,
                    Token = null
                });
            }
        }

        private HttpResponse<UsuarioResponse> newHttpResponse(UsuarioResponse response)
        {
            return new HttpResponse<UsuarioResponse>()
            {
                StatusCode = HttpStatusCode.OK,
                ApiResponse = new ApiResponse<UsuarioResponse>()
                {
                    Data = response,
                    Error = null
                }
            };
        }

        private HttpResponse<UsuarioResponse> newHttpErrorResponse(Error error)
        {
            return new HttpResponse<UsuarioResponse>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ApiResponse = new ApiResponse<UsuarioResponse>()
                {
                    Data = null,
                    Error = error
                }
            };
        }
    }
}