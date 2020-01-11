using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Gevi.Api.Models;
using Gevi.Api.Middleware;

namespace Gevi.Api.Controllers
{
    [Authorize]
    [RoutePrefix("usuarios")]
    public class UsuariosController : ApiController
    {
    //    private GeviApiContext db = new GeviApiContext();

    //    [Route("{id}")]
    //    public async Task<IHttpActionResult> GetUsuario(int id)
    //    {
    //        Usuario usuario = await db.Usuarios.FindAsync(id);
    //        if (usuario == null)
    //        {
    //            return this.Content(HttpStatusCode.NotFound, new HttpResponse<Usuario>()
    //            {
    //                StatusCode = 404,
    //                ApiResponse = new ApiResponse<Usuario>()
    //                {
    //                    Data = null,
    //                    Error = new Error("El usuario no existe")
    //                }
    //            });
    //        }
            
    //        return this.Content(HttpStatusCode.OK, new HttpResponse<Usuario>()
    //        {
    //            StatusCode = 200,
    //            ApiResponse = new ApiResponse<Usuario>()
    //            {
    //                Data = usuario,
    //                Error = null
    //            }
    //        });
    //    }

    //    public async Task<IHttpActionResult> PutUsuario(int id, Usuario usuario)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return BadRequest(ModelState);
    //        }

    //        if (id != usuario.Id)
    //        {
    //            return BadRequest();
    //        }

    //        db.Entry(usuario).State = EntityState.Modified;

    //        try
    //        {
    //            await db.SaveChangesAsync();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!UsuarioExists(id))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }
    //        return StatusCode(HttpStatusCode.NoContent);
    //    }

    //    [HttpPost]
    //    [Route("nuevo")]
    //    public async Task<IHttpActionResult> NuevoUsuario(UsuarioRequest usuario)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            return this.Content(HttpStatusCode.BadRequest, new HttpResponse<string>()
    //            {
    //                StatusCode = 400,
    //                ApiResponse = new ApiResponse<string>()
    //                {
    //                    Data = null,
    //                    Error = new Error("El usuario que se intenta ingresar es invalido")
    //                }
    //            });
    //        }
    //        Usuario nuevo = null;
    //        switch (usuario.EsEmpleado)
    //        {
    //            case true:
    //                nuevo = new Empleado()
    //                {
    //                    Email = usuario.Email,
    //                    Contrasenia = usuario.Contrasenia,
    //                    Nombre = usuario.Nombre,
    //                    FechaRegistro = DateTime.Now,
    //                    Viajes = null
    //                };
    //                break;
    //            case false:
    //                nuevo = new Administrativo()
    //                {
    //                    Email = usuario.Email,
    //                    Contrasenia = usuario.Contrasenia,
    //                    Nombre = usuario.Nombre,
    //                    FechaRegistro = DateTime.Now
    //                };
    //                break;
    //            default:
    //                break;
    //        }
    //        var encryptionManager = new EncryptionManager();
    //        nuevo.Contrasenia = encryptionManager.Encryptdata(nuevo.Contrasenia);
    //        try
    //        {
    //            db.Usuarios.Add(nuevo);
    //            await db.SaveChangesAsync();
    //        }
    //        catch (DbUpdateException)
    //        {
    //            return this.Content(HttpStatusCode.InternalServerError, new HttpResponse<Error>()
    //            {
    //                StatusCode = 500,
    //                ApiResponse = new ApiResponse<Error>()
    //                {
    //                    Data = null,
    //                    Error = new Error("Ya existe un usuario con ese email.")
    //                }
    //            });
    //        }
            

    //        return this.Content(HttpStatusCode.Created, new HttpResponse<UsuarioResponse>()
    //        {
    //            StatusCode = 201,
    //            ApiResponse = new ApiResponse<UsuarioResponse>()
    //            {
    //                Data = new UsuarioResponse()
    //                {
    //                    Id = db.Usuarios.OrderByDescending(u => u.Id).Select(u => u.Id).FirstOrDefault(),
    //                    Email = nuevo.Email,
    //                    Nombre = nuevo.Nombre,
    //                    FechaRegistro = DateTime.Today,
    //                    Token = null
    //                },
    //                Error = null
    //            }
    //        });
    //    }

    //    // DELETE: api/Usuarios/5
    //    [ResponseType(typeof(Usuario))]
    //    public async Task<IHttpActionResult> DeleteUsuario(int id)
    //    {
    //        Usuario usuario = await db.Usuarios.FindAsync(id);
    //        if (usuario == null)
    //        {
    //            return NotFound();
    //        }

    //        db.Usuarios.Remove(usuario);
    //        await db.SaveChangesAsync();

    //        return Ok(usuario);
    //    }
        
    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            db.Dispose();
    //        }
    //        base.Dispose(disposing);
    //    }

    //    private bool UsuarioExists(int id)
    //    {
    //        return db.Usuarios.Count(e => e.Id == id) > 0;
    //    }
        
    }
}