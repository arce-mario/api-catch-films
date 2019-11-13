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
using System.Collections.Generic;

namespace ApiCatchFilms.Controllers
{
    public class MoviesController : ApiController
    {
        private ApiCatchFilmsContext db = new ApiCatchFilmsContext();

        //opc == 1 // todos los registros
        public IQueryable<Movie> GetMovies(string movie = "",string name="", int opc = 0)
        {
            if (movie != ""){
                return db.Movies
                    .Where(m => m.name.Contains(movie))
                    .Take(2);
            }

            if (name != "")
            {
                return db.Movies.Where(m => db.Functions.Where(f => (m.movieID == f.movieID) && f.time >= DateTime.UtcNow).Count() >= 1 && m.name.Contains(name));
            }

            if (opc == 0)
            {
                return db.Movies.Where(m => (db.Functions.Where(f => (f.movieID == m.movieID) && f.time >= DateTime.UtcNow).Count() >= 1));
            }

            if (opc == 1)
            {
                List<Movie> movies = db.Movies.ToList();
                movies.ForEach(m => m.status = (db.Functions.Where(f => (f.movieID == m.movieID)).Count()));
                return movies.AsQueryable();
            }
            return null;
        }
        // GET: api/Movies/5
        [ResponseType(typeof(Movie))]
        public async Task<IHttpActionResult> GetMovie(int id, int opc = 0)
        {
            Movie movie = await db.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            switch (opc)
            {
                case 0:
                    movie.functions = db.Functions.Where(f => f.movieID == movie.movieID && f.time >= DateTime.UtcNow).ToList();
                    break;
                case 1:
                    movie.functions = db.Functions.Where(f => f.movieID == movie.movieID).ToList();
                    break;
            }

            return Ok(movie);
        }

        [Authorize]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMovie(int id, Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != movie.movieID)
            {
                return BadRequest();
            }

            db.Entry(movie).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
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
        [ResponseType(typeof(Movie))]
        public async Task<IHttpActionResult> PostMovie(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Movies.Add(movie);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = movie.movieID }, movie);
        }
        [Authorize]
        [ResponseType(typeof(Movie))]
        public async Task<IHttpActionResult> DeleteMovie(int id)
        {
            Movie movie = await db.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            else if (db.Functions.Where(fn => fn.movieID == movie.movieID).Count() == 0)
            {
                db.Movies.Remove(movie);
                await db.SaveChangesAsync();
                return Ok(movie);
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

        private bool MovieExists(int id)
        {
            return db.Movies.Count(e => e.movieID == id) > 0;
        }
    }
}