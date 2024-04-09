using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FilmCatalogue.Models
{
    [Table("films")]
    public class Film
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }

        [Column("name")]
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Column("director")]
        [Required]
        public string Director { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [Column("release")]
        [Required]
        public DateTime Release { get; set; }

        [NotMapped]
        public string Categories { get; set; }

        public ICollection<FilmCategory> FilmCategories { get; } = new List<FilmCategory>();
    }
}