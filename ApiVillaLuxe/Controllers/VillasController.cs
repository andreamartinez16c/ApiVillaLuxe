using ApiVillaLuxe.Models;
using ApiVillaLuxe.Repositories;
using ApiVillaLuxeApiVillaLuxe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiVillaLuxe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillasController : ControllerBase
    {
        private RepositoryVillas repo;
        public VillasController(RepositoryVillas repo)
        {
            this.repo = repo;
        }

        [HttpGet("villasunicas")]
        public async Task<ActionResult<List<Villa>>> GetVillasUnicas()
        {
            return await this.repo.GetVillasUnicasAsync();
        }

        [HttpGet("villasunicas/{provincia}")]
        public async Task<ActionResult<List<Villa>>> GetVillasUnicasProvincia(string provincia)
        {
            return await this.repo.GetVillasByProvinciaAsync(provincia);
        }

        [HttpGet]
        [Route("{idvilla}")]
        public async Task<ActionResult<VillaFechasResevadas>> DetallesVilla(int idvilla)    
        {
            VillaFechasResevadas fechasReservadas = new VillaFechasResevadas();
            fechasReservadas.FechasReservadas = await this.repo.GetFechasReservadasByIdVillaAsync(idvilla);
            fechasReservadas.Villa = await this.repo.FindVillaAsync(idvilla);

            if (fechasReservadas.Villa == null)
            {
                return NotFound(); // Devuelve un res ultado NotFound si la villa no se encuentra
            }

            return fechasReservadas; // Devuelve un resultado Ok con los detalles de la villa y las fechas reserv
        }

        [HttpGet("detallesVilla/{idvilla}")]
        public async Task<ActionResult<Villa>> FindVilla(int idvilla)
        {
            var villa = await this.repo.FindVillaAsync(idvilla);
            if (villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        }

        [HttpPost("insertreserva")]
        public async Task<ActionResult> CreateReserva(Reserva reserva)
        {
            int idusuario = 1;
            try
            {
                await repo.CreateReserva(reserva, idusuario);
                return Ok("Reserva creada exitosamente");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /*
                [HttpPost]
                public async Task <ActionResult> ReservarVilla(Reserva reserva)
                {
                    int idusuario = 0;
                    return await this.repo.CreateVillaAsync(reserva, idusuario);
                }

                [HttpGet]
                public async Task<ActionResult> MisReservas()
                {
                    int idusuario;
                    return await this.repo.GetMisReservas(idusuario);
                }*/

        [HttpDelete("{idreserva}")]
        public async Task<ActionResult> DeleteReserva(int idreserva)
        {
            if (await this.repo.FindReserva(idreserva) == null)
            {
                return NotFound();
            }
            else
            {
                await this.repo.DeleteReserva(idreserva);
                return Ok();
            }
        }


        // GET: api/reservas/{idVilla}
        /* [HttpGet("{idVilla}/reservasDOS")]
         public async Task<ActionResult<IEnumerable<Reserva>>> GetReservasByIdVillaAsync(int idVilla)
         {
             var reservas = await repo.GetReservasByIdVillaAsync(idVilla);
             if (reservas == null)
             {
                 return NotFound();
             }
             return Ok(reservas);
         }*/

        // GET: api/reservas/villa/{idVilla}
        [HttpGet("reservasvilla/{idVilla}")]
        public async Task<ActionResult<List<Reserva>>> GetReservasByIdVillaAsync(int idVilla)
        {
            var reservas = await this.repo.GetReservasByIdVillaAsync(idVilla);
            if (reservas == null || reservas.Count == 0)
            {
                return NotFound();
            }
            return Ok(reservas);
        }

        // GET: api/misreservas/usuario/{idUsuario}
        [HttpGet("usuario/{idUsuario}")]
        public async Task<ActionResult<List<MisReservas>>> GetMisReservas(int idUsuario)
        {
            var misReservas = await this.repo.GetMisReservas(idUsuario);
            if (misReservas == null || misReservas.Count == 0)
            {
                return NotFound();
            }
            return Ok(misReservas);
        }

        // GET: api/reservas/find/{idReserva}
        [HttpGet("detalles/{idReserva}")]
        public async Task<ActionResult<Reserva>> FindReserva(int idReserva)
        {
            var reserva = await this.repo.FindReserva(idReserva);
            if (reserva == null)
            {
                return NotFound();
            }
            return Ok(reserva);
        }

        [HttpPost("insertvilla")]
        public async Task<ActionResult> CreateVilla(VillaTabla villa)
        {
            VillaTabla villaNew = await this.repo.CreateVillaAsync(villa);
            return Ok(villaNew);
        }

        [HttpPut("editvilla")]
        public async Task<ActionResult> EditVilla(VillaTabla villa)
        {
            await this.repo.EditVilla(villa);
            return Ok("Villa modificada exitosamente");
        }

        [HttpDelete]
        [Route("deletevilla/{idvilla}")]
        public async Task<ActionResult> DeleteVilla(int idvilla)
        {
            await this.repo.DeleteVilla(idvilla);
            return Ok("Villa eliminada exitosamente");
        }

        // GET: api/imagenes/provincias
        [HttpGet("provincias")]
        public async Task<ActionResult<List<Provincias>>> GetProvincias()
        {
            var provincias = await repo.GetProvincias();
            return Ok(provincias);
        }

        // GET: api/villas/provincia/{idProvincia}
        [HttpGet("provincia/{idProvincia}")]
        public async Task<ActionResult<IEnumerable<Villa>>> GetVillasByProvincia(int idProvincia)
        {
            var villas = await this.repo.GetVillasByProvinciaAsync(idProvincia);
            if (villas == null || villas.Count == 0)
            {
                return NotFound(); // Si no se encuentran villas para la provincia especificada, devolver 404 Not Found
            }
            return Ok(villas); // Devolver las villas encontradas
        }

        // GET: api/imagenes/villa/5
        [HttpGet("imagenesvilla/{idvilla}")]
        public async Task<ActionResult<List<Imagen>>> GetImagenesVilla(int idvilla)
        {
            var imagenes = await repo.GetImagenesVilla(idvilla);
            return Ok(imagenes);
        }

        // GET: api/imagenes/5
        [HttpGet("imagen/{idimagen}")]
        public async Task<ActionResult<Imagen>> GetImagen(int idimagen)
        {
            var imagen = await repo.FindImagenVilla(idimagen);

            if (imagen == null)
            {
                return NotFound();
            }

            return imagen;
        }

        // DELETE: api/imagenes/5
        [HttpDelete("imagen/{idimagen}")]
        public async Task<ActionResult> DeleteImagen(int idimagen)
        {
            await repo.DeleteImagenes(idimagen);

            return NoContent();
        }

        // POST: api/imagenes
        [HttpPost("insertimagen/{idvilla}/{url}")]
        public async Task<ActionResult<Imagen>> InsertarImagen(int idvilla, string url)
        {
            try
            {
                await repo.InsertarImagenes(idvilla, url);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
