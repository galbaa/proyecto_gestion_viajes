using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Gevi.Api.Models;

namespace Gevi.Api.Controllers
{
    public class UsuariosController : ApiController
    {
        private GeviApiContext db = new GeviApiContext();

        [Route("usuarios")]
        public async Task<IHttpActionResult> GetUsuarios()
        {
            var usuarios = db.Usuarios;
            if (usuarios == null)
                return this.Content(HttpStatusCode.NotFound, new HttpResponse<IQueryable<Usuario>>()
                {
                    StatusCode = 404,
                    ApiResponse = new ApiResponse<IQueryable<Usuario>>()
                    {
                        Data = null,
                        Error = new Error("Error en la llamada")
                    }
                });

            return Ok(new HttpResponse<IQueryable<Usuario>>()
            {
                StatusCode = 200,
                ApiResponse = new ApiResponse<IQueryable<Usuario>>()
                {
                    Data = usuarios,
                    Error = null
                }
            });
        }


        [Route("usuarios/{id}")]
        public async Task<IHttpActionResult> GetUsuario(int id)
        {
            Usuario usuario = await db.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return this.Content(HttpStatusCode.NotFound, new HttpResponse<Usuario>()
                {
                    StatusCode = 404,
                    ApiResponse = new ApiResponse<Usuario>()
                    {
                        Data = null,
                        Error = new Error("El usuario no existe")
                    }
                });
            }
            
            return Ok(new HttpResponse<Usuario>()
            {
                StatusCode = 200,
                ApiResponse = new ApiResponse<Usuario>()
                {
                    Data = usuario,
                    Error = null
                }
            });
        }

        // PUT: api/Usuarios/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuario.Id)
            {
                return BadRequest();
            }

            db.Entry(usuario).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Usuarios
        [Route("usuarios/nuevo")]
        public async Task<IHttpActionResult> PostUsuario(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return this.Content(HttpStatusCode.BadRequest, new HttpResponse<string>()
                {
                    StatusCode = 400,
                    ApiResponse = new ApiResponse<string>()
                    {
                        Data = null,
                        Error = new Error("El usuario que se intenta ingresar es invalido")
                    }
                });
            }
            usuario.FechaRegistro = DateTime.Today;
            db.Usuarios.Add(usuario);
            await db.SaveChangesAsync();

            return this.Content(HttpStatusCode.Created, new HttpResponse<string>()
            {
                StatusCode = 201,
                ApiResponse = new ApiResponse<string>()
                {
                    Data = "Usuario ingresado correctamente",
                    Error = null
                }
            });
        }

        // DELETE: api/Usuarios/5
        [ResponseType(typeof(Usuario))]
        public async Task<IHttpActionResult> DeleteUsuario(int id)
        {
            Usuario usuario = await db.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            db.Usuarios.Remove(usuario);
            await db.SaveChangesAsync();

            return Ok(usuario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuarioExists(int id)
        {
            return db.Usuarios.Count(e => e.Id == id) > 0;
        }
    }
}