using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCatchFilms.Models
{
    public class Function
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("function_id")]
        public int functionID { get; set; }
        [Required]
        [Column("movie_id")]
        public int movieID { get; set; }
        [Required]
        [Column("room_id")]
        public int roomID { get; set; }
        [Required]
        [Column("price_id")]
        public int priceID { get; set; }
        [Required]
        public DateTime time { get; set; }
        public int type { get; set; }
        [Column("type_movie")]
        [Required]
        public int typeMovie { get; set; }
        [StringLength(500)]
        public string description { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Movie movie { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Price price { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Room room { get; set; }
    }
}