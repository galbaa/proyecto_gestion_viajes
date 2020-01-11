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
    public class ViajesController : ApiController
    {
        /*private GeviApiContext db = new GeviApiContext();

        // GET: api/Viajes
        public IQueryable<Viaje> GetViajes()
        {
            return db.Viajes;
        }

        // GET: api/Viajes/5
        [ResponseType(typeof(Viaje))]
        public async Task<IHttpActionResult> GetViaje(int id)
        {
            Viaje viaje = await db.Viajes.FindAsync(id);
            if (viaje == null)
            {
                return NotFound();
            }

            return Ok(viaje);
        }

        // PUT: api/Viajes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutViaje(int id, Viaje viaje)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != viaje.Id)
            {
                return BadRequest();
            }

            db.Entry(viaje).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ViajeExists(id))
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

        [HttpPost]
        [Route("nuevo")]
        public async Task<IHttpActionResult> NuevoViaje(ViajeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.Content(HttpStatusCode.BadRequest, new HttpResponse<string>()
                {
                    StatusCode = 400,
                    ApiResponse = new ApiResponse<string>()
                    {
                        Data = null,
                        Error = new Error("El viaje que se intenta ingresar es invalido")
                    }
                });
            }
            var empleado = (Empleado)db.Usuarios.Where(u => u is Empleado && u.Id == request.EmpleadoId).FirstOrDefault();
            var nuevo = new Viaje()
            {
                Empleado = empleado,
                Estado = Estado.PENDIENTE_APROBACION,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                Gastos = null,
                Proyecto = null
            };
            db.Viajes.Add(nuevo);
            await db.SaveChangesAsync();

            return this.Content(HttpStatusCode.Created, new HttpResponse<Viaje>()
            {
                StatusCode = 201,
                ApiResponse = new ApiResponse<Viaje>()
                {
                    Data = nuevo,
                    Error = null
                }
            });
        }

        // DELETE: api/Viajes/5
        [ResponseType(typeof(Viaje))]
        public async Task<IHttpActionResult> DeleteViaje(int id)
        {
            Viaje viaje = await db.Viajes.FindAsync(id);
            if (viaje == null)
            {
                return NotFound();
            }

            db.Viajes.Remove(viaje);
            await db.SaveChangesAsync();

            return Ok(viaje);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ViajeExists(int id)
        {
            return db.Viajes.Count(e => e.Id == id) > 0;
        }*/
    }
}