using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FilmCatalogue.Models
{
    [Table("Categories")]
    public class Category
    {
        [Column("Id")]
        [Key]
        public int Id { get; set; }

        [Column("name")]
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Column("parent_category_id")]
        public int? ParentCategoryId { get; set; }

        public Category ParentCategory { get; set; }
        public ICollection<Category> Subcategories { get; } = new List<Category>();
        public ICollection<FilmCategory> FilmCategories { get; } = new List<FilmCategory>();
    }
}