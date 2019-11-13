using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApiCatchFilms.Models
{
    [Table("Room_seat")]
    public class RoomSeat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("room_seat_id")]
        public int roomSeatID { get; set; }
        [Required]
        public int status { get; set; }
        [Required]
        [Column("seat_id")]
        public int seatID { get; set; }
        [Column("room_id")]
        public int roomID { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Room room { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Seat seat { get; set; }
    }
}