using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApiCatchFilms.Models
{
    public class Ticket
    {
        [Key]
        [Column("ticket_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ticketID { get; set; }
        [Required]
        [Column("create_at")]
        public DateTime createAT { get; set; }
        [Column("room_seat_id")]
        public int roomSeatID { get; set; }
        [Required]
        [Column("price_id")]
        public int priceID { get; set; }
        [Required]
        [Column("user_id")]
        public int userID { get; set; }
        [Required]
        [Column("function_id")]
        public int functionID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RoomSeat roomSeat { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Function function { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Price price { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public User user { get; set; }
    }
}