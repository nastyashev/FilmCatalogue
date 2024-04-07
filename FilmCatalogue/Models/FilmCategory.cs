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
        [ForeignKey(nameof(Film))]
        public int FilmId { get; set; }

        [Column("category_id")]
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public virtual Film Film { get; set; }
        public virtual Category Category { get; set; }
    }
}