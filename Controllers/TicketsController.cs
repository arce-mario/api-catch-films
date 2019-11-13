using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ApiCatchFilms.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace ApiCatchFilms.Controllers
{
    [Authorize]
    public class TicketsController : ApiController
    {
        private ApiCatchFilmsContext db = new ApiCatchFilmsContext();
        
        public IQueryable<Ticket> GetTickets()
        {
            return db.Tickets.Include(t => t.price).Include(t => t.user);
        }
        
        [ResponseType(typeof(Ticket))]
        public async Task<IHttpActionResult> GetTicket(int id)
        {
            Ticket ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }
        
        public async Task<IHttpActionResult> PutTicket(int id, Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ticket.ticketID)
            {
                return BadRequest();
            }

            db.Entry(ticket).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
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
        
        public IHttpActionResult PostTicket(Ticket ticket, string notAvailable = "")
        {
            List<Ticket> tickets = new List<Ticket>();
            List<string> seats = JsonConvert.DeserializeObject<List<string>>(notAvailable);
            List<RoomSeat> roomSeats = new List<RoomSeat>();

            foreach (string identity in seats)
            {
                string[] seatData = identity.Split('_');
                int cdata = int.Parse(seatData[1]);
                string rdata = seatData[0];

                RoomSeat roomSeat =  db.RoomSeats
                    .Include(r => r.seat)
                    .Where(r =>
                       r.seat.column == cdata &&
                       r.seat.row == rdata
                    ).FirstOrDefault();

                if (roomSeat != null)
                {
                    roomSeat.status = (roomSeat.status == 1) ? 2 : 1;
                    db.Entry(roomSeat).State = EntityState.Modified;
                    db.SaveChanges();
                    roomSeats.Add(roomSeat);
                }
            }

            Debug.WriteLine("Tickets: " + JsonConvert.SerializeObject(ticket));

            foreach (RoomSeat roomseat in roomSeats)
            {
                tickets.Add(new Ticket() {
                    createAT = DateTime.UtcNow,
                    functionID = ticket.functionID,
                    priceID = ticket.priceID,
                    roomSeatID = roomseat.roomSeatID,
                    userID = 1002
                });
            }

            Debug.WriteLine("Tickets: "+JsonConvert.SerializeObject(tickets));

            try
            {
                db.Tickets.AddRange(tickets);
                db.SaveChanges();

            }
            catch (Exception e)
            {
                Debug.WriteLine("ERROR: "+e.Message);
            }
            return CreatedAtRoute("DefaultApi", new { id = tickets.FirstOrDefault().ticketID }, tickets.FirstOrDefault());
        }

        // DELETE: api/Tickets/5
        [ResponseType(typeof(Ticket))]
        public async Task<IHttpActionResult> DeleteTicket(int id)
        {
            Ticket ticket = await db.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            db.Tickets.Remove(ticket);
            await db.SaveChangesAsync();

            return Ok(ticket);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TicketExists(int id)
        {
            return db.Tickets.Count(e => e.ticketID == id) > 0;
        }
    }
}