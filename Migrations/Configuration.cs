namespace ApiCatchFilms.Migrations
{
    using ApiCatchFilms.Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApiCatchFilms.Models.ApiCatchFilmsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "ApiCatchFilms.Models.ApiCatchFilmsContext";
        }

        protected override void Seed(ApiCatchFilms.Models.ApiCatchFilmsContext context)
        {
            context.Users.AddOrUpdate(u => u.userID, new User()
            {
                firstName = "Roberto",
                lastName = "Rodr�guez",
                email = "roberto@gmail.com",
                hireDare = new DateTime(2019, 8, 4),
                userName = "Administrador",
                pass = "Usuario123.",
                birthDate = new DateTime(1999, 05, 29),
                rol = 1
            });
            context.SaveChanges();

            context.Movies.AddOrUpdate(m => m.movieID, new Movie()
            {
                name = "Buscando a Dory",
                type = "Infatil",
                description = "Con la ayuda de Nemo y Marlin, Dory, el olvidadizo pez, se embarca en la misi�n de reunirse con su madre y padre.",
                classification = "AA",
                time = new TimeSpan(01, 30, 00),
                status = 1,
                coverURL = "",
                imageURL = "FindingDory.jpg",
                rating = 4,
            });
            context.SaveChanges();
            context.Movies.AddOrUpdate(m => m.movieID, new Movie()
            {
                name = "Love Rosie",
                type = "Drama/Romance",
                description = "Rosie y Alex son mejores amigos hasta que la familia se muda a Estados Unidos. Ellos se juegan todo para conservar vivos su amor y amistad al paso de los a�os y las millas.",
                classification = "B",
                time = new TimeSpan(01, 30, 00),
                status = 1,
                coverURL = "",
                imageURL = "LoveRosie.jpg",
                rating = 5,
            });
            context.SaveChanges();
            context.Movies.AddOrUpdate(m => m.movieID, new Movie()
            {
                name = "La milla verde",
                type = "Drama/Fantas�a",
                description = "El guardia de una prisi�n descubre que un preso posee un milagroso poder de curaci�n.",
                classification = "B15",
                time = new TimeSpan(01, 30, 00),
                status = 1,
                coverURL = "",
                imageURL = "GreenMile.jpg",
                rating = 4,
            });
            context.SaveChanges();
            context.Movies.AddOrUpdate(m => m.movieID, new Movie()
            {
                name = "Cuesti�n de tiempo",
                type = "Drama/Fantas�a",
                description = "Cuando Tim Lake cumple 21 a�os, su padre le dice un secreto: los hombres de su familia pueden viajar por el tiempo. A pesar de que �l no puede cambiar la historia, Tim decide mejorar su vida buscando una novia.",
                classification = "B",
                time = new TimeSpan(02, 04, 00),
                status = 1,
                coverURL = "",
                imageURL = "AboutTime.jpg",
                rating = 4,
            });
            context.SaveChanges();
            context.Movies.AddOrUpdate(m => m.movieID, new Movie()
            {
                name = "The Hunger Games",
                type = "Fantas�a",
                description = "La Capital de Panem mantiene sus 12 distritos oblig�ndolos a seleccionar a un ni�o y a una ni�a, llamados Tributos, a competir en un evento televisado nacionalmente llamado los Juegos del Hambre. Cada ciudadano debe ver pelear a muerte a los j�venes.",
                classification = "PG-13",
                time = new TimeSpan(01, 30, 00),
                status = 1,
                coverURL = "",
                imageURL = "HungerGames.jpg",
                rating = 4,
            });
            context.SaveChanges();
            context.Movies.AddOrUpdate(m => m.movieID, new Movie()
            {
                name = "This Is Us",
                type = "Musical",
                description = "Los miembros de One Direction hablan sobre sus inicios en su ciudad natal, su aparici�n en The X - Factor y su mete�rico ascenso a la fama.",
                classification = "A",
                time = new TimeSpan(01, 46, 00),
                status = 1,
                coverURL = "",
                imageURL = "1D.jpg",
                rating = 4,
            });
            context.SaveChanges();
            context.Movies.AddOrUpdate(m => m.movieID, new Movie()
            {
                name = "Desde mi cielo",
                type = "Drama/Pel�cula de suspenso",
                description = "Despu�s de que es violada y asesinada, una joven de 14 a�os vigila desde el cielo mientras su familia intenta superar lo acontecido y el homicida contin�a con su oscuro camino.",
                classification = "PG-13 ",
                time = new TimeSpan(02, 15, 00),
                status = 1,
                coverURL = "",
                imageURL = "LovelyBone.jpg",
                rating = 5,
            });
            context.SaveChanges();
            context.Rooms.AddOrUpdate(r => r.roomID, new Room()
            {
                number = 1,
                description = "Proyector de EPSON FULLHD"
            });
            context.SaveChanges();

            context.Seats.AddOrUpdate(s => s.seatID, new Seat()
            {
                column = 1,
                row = "A"
            });
            context.SaveChanges();

            try
            {
                int lastSeatID = context.Seats.OrderByDescending(s => s.seatID).First().seatID;
                int lastRoomID = context.Rooms.OrderByDescending(r => r.roomID).First().roomID;

                context.RoomSeats.AddOrUpdate(rs => rs.roomSeatID, new RoomSeat
                {
                    roomID = lastRoomID,
                    seatID = lastSeatID,
                    status = 1
                });
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.WriteLine(String.Concat("Ha ocurrido un error", e.Message));
            }
        }
    }
}