using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ApiCatchFilms.Models;
using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace ApiCatchFilms.Controllers
{
    public class FunctionsController : ApiController
    {
        private ApiCatchFilmsContext db = new ApiCatchFilmsContext();

        // GET: api/Functions
        public IQueryable<Function> GetFunctions()
        {
            return db.Functions;
        }

        // GET: api/Functions/5
        [ResponseType(typeof(Function))]
        public async Task<IHttpActionResult> GetFunction(int id)
        {
            Function function = await db.Functions.Include(f => f.movie)
                .Include(f => f.price).Include(f => f.room)
                .FirstOrDefaultAsync(f => f.functionID == id);
            if (function.room != null)
            {
                function.room.seatAvalaibles = db.RoomSeats.Where(rs=> rs.roomID == function.roomID && rs.status == 1).Count();
                function.room.seatNotAvalaibles = db.RoomSeats.Where(rs => rs.roomID == function.roomID && rs.status == 2).Count();
            }
            if (function == null)
            {
                return NotFound();
            }

            return Ok(function);
        }
        // GET: api/Functions/getSimpleFunction/1
        [Route("api/functions/simplemodel/{id}")]
        [ResponseType(typeof(Function))]
        public async Task<IHttpActionResult> GetSimpleModel(int id)
        {
            Function function = await db.Functions.FindAsync(id);
            if (function == null)
            {
                return NotFound();
            }

            return Ok(function);
        }
        
        // GET: api/Functions/getSimpleFunction/1
        [Route("api/functions/movie/{movieID:int}")]
        public IQueryable<Function> GetFunctionsMovie(int movieID)
        {
            return db.Functions
                .Include(f => f.movie)
                .Include(f => f.price).Include(f => f.room)
                .Where(f => (f.movieID == movieID && f.time >= DateTime.UtcNow));
        }

        [Authorize]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFunction(int id, Function function)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != function.functionID)
            {
                return BadRequest();
            }

            db.Entry(function).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FunctionExists(id))
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

        [Authorize]
        [ResponseType(typeof(Function))]
        public async Task<IHttpActionResult> PostFunction(Function function)
        {
            if (function.priceID > 0)
            {
                function.price = null;
            }
            if (function.movieID > 0)
            {
                function.movie = null;
            }
            if (function.roomID > 0)
            {
                function.room = null;
            }
            //Se valida que el usuario no haya ingresado un valor existente en la base de datos
             if(function.priceID == -1)
             {
                Price price = await db.Prices.Where(p => p.valid != null).FirstOrDefaultAsync();
                Debug.WriteLine("FunctionController :: PostFunction() :: default price: " + JsonConvert.SerializeObject(price));
                if (price == null ) { return StatusCode(HttpStatusCode.NotAcceptable); }
                function.priceID = price.priceID;
            }
            Debug.WriteLine("Función: " + JsonConvert.SerializeObject(function));

            db.Functions.Add(function);
            await db.SaveChangesAsync();
            return CreatedAtRoute("DefaultApi", new { id = function.functionID }, function);
        }

        [Authorize]
        [ResponseType(typeof(Function))]
        public async Task<IHttpActionResult> DeleteFunction(int id)
        {
            Function function = await db.Functions.FindAsync(id);
            if (function == null)
            {
                return NotFound();
            }

            db.Functions.Remove(function);
            await db.SaveChangesAsync();

            return Ok(function);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FunctionExists(int id)
        {
            return db.Functions.Count(e => e.functionID == id) > 0;
        }
    }
}