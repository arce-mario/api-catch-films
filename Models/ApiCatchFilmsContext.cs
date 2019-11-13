using System.Data.Entity;

namespace ApiCatchFilms.Models
{
    public class ApiCatchFilmsContext : DbContext
    {
        public System.Data.Entity.DbSet<ApiCatchFilms.Models.Function> Functions { get; set; }
        public System.Data.Entity.DbSet<ApiCatchFilms.Models.Movie> Movies { get; set; }
        public System.Data.Entity.DbSet<ApiCatchFilms.Models.Price> Prices { get; set; }
        public System.Data.Entity.DbSet<ApiCatchFilms.Models.Room> Rooms { get; set; }
        public System.Data.Entity.DbSet<ApiCatchFilms.Models.Seat> Seats { get; set; }
        public System.Data.Entity.DbSet<ApiCatchFilms.Models.RoomSeat> RoomSeats { get; set; }
        public System.Data.Entity.DbSet<ApiCatchFilms.Models.Ticket> Tickets { get; set; }
        public System.Data.Entity.DbSet<ApiCatchFilms.Models.User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>().HasRequired(p => p.price).WithMany().HasForeignKey(p => p.priceID).WillCascadeOnDelete(false);
            modelBuilder.Entity<Ticket>().HasRequired(p => p.roomSeat).WithMany().HasForeignKey(p => p.roomSeatID).WillCascadeOnDelete(false);
        }
    }
}
