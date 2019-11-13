using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ApiCatchFilms.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("movie_id")]
        public int movieID { get; set; }
        [Required]
        [StringLength(30)]
        public string name { get; set; }
        [Required]
        [StringLength(30)]
        public String type { get; set; }
        [Required]
        [StringLength(500)]
        public String description { get; set; }
        [Required]
        [StringLength(30)]
        public string classification { get; set; }
        [Required]
        public TimeSpan time { get; set; }
        [Required]
        public int status { get; set; }
        [StringLength(800)]
        [Column("cover_url")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string coverURL { get; set; }
        [StringLength(800)]
        [Column("image_url")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string imageURL { get; set; }
        [Required]
        public float rating { get; set; }
        [NotMapped]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Function> functions { get; set; }
    }
}