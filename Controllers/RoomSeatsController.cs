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
using ApiCatchFilms.Models;

namespace ApiCatchFilms.Controllers
{
    [Authorize]
    public class RoomSeatsController : ApiController
    {
        private ApiCatchFilmsContext db = new ApiCatchFilmsContext();

        // GET: api/RoomSeats
        public IQueryable<RoomSeat> GetRoomSeats()
        {
            return db.RoomSeats.Include(r => r.seat);
        }
        
        [ResponseType(typeof(RoomSeat))]
        public async Task<IHttpActionResult> GetRoomSeat(int id)
        {
            RoomSeat roomSeat = await db.RoomSeats.FindAsync(id);
            if (roomSeat == null)
            {
                return NotFound();
            }

            return Ok(roomSeat);
        }
        
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRoomSeat(int id, RoomSeat roomSeat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != roomSeat.roomSeatID)
            {
                return BadRequest();
            }

            db.Entry(roomSeat).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomSeatExists(id))
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
        
        [ResponseType(typeof(RoomSeat))]
        public async Task<IHttpActionResult> PostRoomSeat(RoomSeat roomSeat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.RoomSeats.Add(roomSeat);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = roomSeat.roomSeatID }, roomSeat);
        }
        
        [ResponseType(typeof(RoomSeat))]
        public async Task<IHttpActionResult> DeleteRoomSeat(int id)
        {
            RoomSeat roomSeat = await db.RoomSeats.FindAsync(id);
            if (roomSeat == null)
            {
                return NotFound();
            }

            db.RoomSeats.Remove(roomSeat);
            await db.SaveChangesAsync();

            return Ok(roomSeat);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RoomSeatExists(int id)
        {
            return db.RoomSeats.Count(e => e.roomSeatID == id) > 0;
        }
    }
}