using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using FilmCatalogue.Models;

namespace FilmCatalogue.Data
{
    public class FilmCatalogueContext : DbContext
    {
        public FilmCatalogueContext() : base("name=FilmCatalogueContext")
        {
        }

        public DbSet<Film> Films { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FilmCategory> FilmCategories { get; set; }
    }
}
