using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCatchFilms.Models
{
    public class Price
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("price_id")]
        public int priceID { get; set; }
        [Required]
        [Column("adult_price")]
        public decimal adultPrice { set; get; }
        [Required]
        [Column("child_price")]
        public decimal childPrice { get; set; }
        [Required]
        [Column("old_man_price")]
        public decimal oldManPrice { get; set; }
        public bool? valid { get; set;}
    }
}