using Gevi.Api.Middleware.Interfaces;
using Gevi.Api.Models;
using Nancy;
using System;
using System.Data.Entity;
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
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    return newHttpErrorResponse(new Error("Ya existe un usuario con ese email."));
                }

                var response = new UsuarioResponse()
                {
                    Id = nuevo.Id,
                    Email = nuevo.Email,
                    Nombre = nuevo.Nombre,
                    EsEmpleado = nuevo is Empleado,
                    FechaRegistro = DateTime.Today
                };

                return newHttpResponse(response);
            }
        }

        public HttpResponse<UsuarioResponse> ModificarUsuario(UsuarioRequest request)
        {
            if (request == null)
                return newHttpErrorResponse(new Error("El usuario que se intenta modificar es invalido."));

            using (var db = new GeviApiContext())
            {
                var usuario = db.Usuarios
                                .Where(u => u.Id == request.UsuarioId)
                                .FirstOrDefault();

                if (usuario == null)
                    return newHttpErrorResponse(new Error("No existe el usuario"));

                usuario.Nombre = request.Nombre;
                usuario.Email = request.Email;

                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();

                var response = new UsuarioResponse()
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Email = usuario.Email,
                    EsEmpleado = usuario is Empleado,
                    FechaRegistro = usuario.FechaRegistro
                };

                return newHttpResponse(response);
            }
        }

        public HttpResponse<UsuarioResponse> CambiarContrasenia(UsuarioRequest request)
        {
            if (request == null)
                return newHttpErrorResponse(new Error("El usuario que se intenta modificar es invalido."));

            using (var db = new GeviApiContext())
            {
                var usuario = db.Usuarios
                                    .Where(u => u.Id == request.UsuarioId)
                                    .FirstOrDefault();

                if (usuario == null)
                    return newHttpErrorResponse(new Error("No existe el usuario"));

                var encryptionManager = new EncryptionManager();
                usuario.Contrasenia = encryptionManager.Encryptdata(request.Contrasenia);

                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();

                var response = new UsuarioResponse()
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Email = usuario.Email,
                    EsEmpleado = usuario is Empleado,
                    FechaRegistro = usuario.FechaRegistro
                };

                return newHttpResponse(response);
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