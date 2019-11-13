using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCatchFilms.Models
{
    public class User
    {
        [Column("user_id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userID { get; set; }
        [Column("first_name")]
        [StringLength(30)]
        [Required]
        public string firstName { get; set; }
        [StringLength(30)]
        [Column("last_name")]
        [Required]
        public string lastName { get; set; }
        [StringLength(40)]
        [Required]
        public string email { get; set; }
        [Column("hire_date")]
        public DateTime hireDare { get; set; }
        [StringLength(30)]
        [Column("user_name")]
        [Required]
        public string userName { get; set; }
        [StringLength(30)]
        [Required]
        public string pass { get; set; }
        [Column("birth_date")]
        [Required]
        public DateTime birthDate { get; set; }
        [Required]
        public int rol { get; set; }
        [StringLength(800)]
        [Column("image_user_url")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string imageUserURL { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<Ticket> tickets { get; set; }
    }
}