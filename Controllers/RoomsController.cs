using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ApiCatchFilms.Models;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApiCatchFilms.Controllers
{
    public class RoomsController : ApiController
    {
        private ApiCatchFilmsContext db = new ApiCatchFilmsContext();

        public IQueryable<Room> GetRooms(string name = "", int opc = 0)
        {
            if (name != "")
            {
                Regex re = new Regex(@"\d+");
                Match m = re.Match(name);

                if (m.Success)
                {
                    int value = int.Parse(m.Value);
                    return db.Rooms.Where(rm => rm.number == value && db.Functions.Where(f => f.roomID == rm.roomID && f.time >= DateTime.UtcNow).Count() == 0);
                }
                
            }

            if (opc == 0)
            {
                return db.Rooms;
            }

            return null;
        }

        [Route("{id:int}/functions")]
        [ResponseType(typeof(IQueryable<Function>))]
        [HttpGet]
        public IQueryable<Function> GetFunctions(int id)
        {
            return db.Functions.Where(f => (f.functionID == id && f.time >= DateTime.UtcNow));
        }
        // GET: api/Rooms/5
        [ResponseType(typeof(Room))]
        public async Task<IHttpActionResult> GetRoom(int id)
        {
            Room room = await db.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            room.roomSeats = db.RoomSeats.Where(r => r.roomID == room.roomID).Include(r => r.seat).ToList();
            return Ok(room);
        }

        [Authorize]
        [ResponseType(typeof(void))]
        [HttpPut]
        public async Task<IHttpActionResult> PutRoom(int id, Room room, string notAvailable = "")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != room.roomID)
            {
                return BadRequest();
            }

            db.Entry(room).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
                if (notAvailable != "")
                {
                    bool result = false;//changeSeatsStatus(notAvailable);

                    if (!result)
                    {
                        return StatusCode(HttpStatusCode.NotAcceptable);
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
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

        private bool changeSeatsStatus(string notAvailable)
        {
            List<string> seats = JsonConvert.DeserializeObject<List<string>>(notAvailable);
            bool result = true;
            foreach (string identity in seats)
            {
                string[] seatData = identity.Split('_');
                int cdata = int.Parse(seatData[1]);
                string rdata = seatData[0];

                RoomSeat roomSeat = db.RoomSeats
                    .Include(r => r.seat)
                    .Where( r =>
                        r.seat.column == cdata &&
                        r.seat.row == rdata &&
                        db.Tickets.Where(t => 
                            t.roomSeatID == r.roomSeatID &&
                            db.Functions.Where(f => f.functionID == t.functionID && f.time >= DateTime.UtcNow).Count() == 0
                        ).Count() == 0
                    ).FirstOrDefault();

                if (roomSeat != null)
                {
                    roomSeat.status = (roomSeat.status == 1) ? 2 : 1;
                    db.Entry(roomSeat).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }
        
        [Authorize]
        [HttpDelete]
        [ResponseType(typeof(Room))]
        public async Task<IHttpActionResult> DeleteRoom(int id)
        {
            Room room = await db.Rooms.FindAsync(id);
            
            if (room == null)
            {
                return NotFound();
            }
            else if(db.Functions.Where(fn => fn.roomID == room.roomID).Count() == 0)
            {
                db.RoomSeats.RemoveRange(db.RoomSeats.Where(rs => rs.roomID == room.roomID));
                db.Rooms.Remove(room);
                await db.SaveChangesAsync();
                return Ok(room);
            }
            return StatusCode(HttpStatusCode.NotAcceptable);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RoomExists(int id)
        {
            return db.Rooms.Count(e => e.roomID == id) > 0;
        }

        private List<Seat> getSeats(string parameterSeats)
        {
            int primaryASCII = 65;
            List<string> seats = JsonConvert.DeserializeObject<List<string>>(parameterSeats);
            List<Seat> listSeats = new List<Seat>();

            for (int i = 0; i < seats.LongCount(); i++)
            {
                string rowTemp = Convert.ToChar(primaryASCII++).ToString();
                for (int c = 0; c < seats.ElementAt(i).Length; c++)
                {
                    listSeats.Add(new Seat() { row = rowTemp, column = (c + 1) });
                }
            }
            return listSeats;
        }

        [Authorize]
        [ResponseType(typeof(Room))]
        [HttpPost]
        public async Task<IHttpActionResult> PostRoom(Room room, string listSeats = "")
        {
            List<Seat> seats = getSeats(listSeats);
            List<RoomSeat> roomSeats = new List<RoomSeat>();
            List<Seat> seatsSave = new List<Seat>();

            if (!ModelState.IsValid || seats == null)
            {
                return BadRequest(ModelState);
            }

            db.Rooms.Add(room);
            await db.SaveChangesAsync();
            List<Seat> seatsDB = await db.Seats.OrderBy(s => s.seatID).ToListAsync();

            int max = seatsDB.Count();
            int min = seats.Count();

            if (min > max)
            {
                int columns = seatsDB.Where(s => s.row == "A").Count();
                int rows = seatsDB.Where(s => s.column == 1).Count();

                seatsSave = seats.Where(s => (int)s.row.ToCharArray()[0] > (rows + 64) || s.column > columns).ToList();
                //Guardamos los registros de nuevas butacas
                db.Seats.AddRange(seatsSave);
                await db.SaveChangesAsync();
            }
            else
            {
                int columns = seats.Where(s => s.row == "A").Count();
                int rows = seats.Where(s => s.column == 1).Count();

                seatsSave = seatsDB.Where(s => (int)s.row.ToCharArray()[0] <= (rows + 64) && s.column <= columns).ToList();
            }
            foreach (Seat seat in seatsSave)
            {
                roomSeats.Add(new RoomSeat() { roomID = room.roomID, seatID = seat.seatID, status = 1 });
            }
            db.RoomSeats.AddRange(roomSeats);
            await db.SaveChangesAsync();
            return CreatedAtRoute("DefaultApi", new { id = room.roomID }, room);
        }

    }
}