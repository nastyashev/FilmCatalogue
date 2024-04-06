using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FilmCatalogue.Models
{
    [Table("film_categories")]
    public class FilmCategory
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }

        [Column("film_id")]
        public int FilmId { get; set; }

        [Column("category_id")]
        public int CategoryId { get; set; }
    }
}