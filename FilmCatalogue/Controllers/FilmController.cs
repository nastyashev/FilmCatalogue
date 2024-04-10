using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FilmCatalogue.Data;
using FilmCatalogue.Models;

namespace FilmCatalogue.Controllers
{
    public class FilmController : Controller
    {
        private readonly FilmCatalogueContext db = new FilmCatalogueContext();

        // GET: Film
        public async Task<ActionResult> Index()
        {
            var query = await db.Films
                .Select(f => new
                {
                    f.Id,
                    f.Name,
                    f.Director,
                    f.Release,
                    Categories = f.FilmCategories.Select(fc => fc.Category.Name)
                })
                .ToListAsync();

            var result = query.Select(f => new Film
            {
                Id = f.Id,
                Name = f.Name,
                Director = f.Director,
                Release = f.Release,
                Categories = string.Join(", ", f.Categories)
            }).ToList();

            return View(result);
        }

        // GET: Film/Create
        public ActionResult Create()
        {
            ViewBag.Categories = db.Categories.ToList();
            return View();
        }

        // POST: Film/Create
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Director,Release")] Film film, int[] categoryIds)
        {
            if (ModelState.IsValid)
            {
                if (categoryIds != null)
                {
                    var categories = await db.Categories.Where(c => categoryIds.Contains(c.Id)).ToListAsync();

                    foreach (var category in categories)
                    {
                        film.FilmCategories.Add(new FilmCategory { Film = film, Category = category });
                    }
                }
                db.Films.Add(film);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(film);
        }

        // GET: Film/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = await db.Films.FindAsync(id);
            if (film == null)
            {
                return HttpNotFound();
            }

            var allCategories = await db.Categories.ToListAsync();
            var filmCategoryIds = await db.FilmCategories
                .Where(fc => fc.FilmId == id)
                .Select(fc => fc.CategoryId)
                .ToListAsync();
            ViewBag.AllCategories = new MultiSelectList(allCategories, "Id", "Name", filmCategoryIds);

            return View(film);
        }

        // POST: Film/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Director,Release")] Film film, int[] categoryIds)
        {
            if (ModelState.IsValid)
            {
                var filmToUpdate = await db.Films.Include(f => f.FilmCategories).FirstOrDefaultAsync(f => f.Id == film.Id);

                if (filmToUpdate != null)
                {
                    filmToUpdate.Name = film.Name;
                    filmToUpdate.Director = film.Director;
                    filmToUpdate.Release = film.Release;

                    var filmCategoriesToRemove = filmToUpdate.FilmCategories.ToList();
                    foreach (var filmCategory in filmCategoriesToRemove)
                    {
                        db.FilmCategories.Remove(filmCategory);
                    }

                    if (categoryIds != null)
                    {
                        var categories = await db.Categories.Where(c => categoryIds.Contains(c.Id)).ToListAsync();

                        foreach (var category in categories)
                        {
                            filmToUpdate.FilmCategories.Add(new FilmCategory { FilmId = film.Id, CategoryId = category.Id });
                        }
                    }

                    db.Entry(filmToUpdate).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return View(film);
        }

        // GET: Film/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = await db.Films.FindAsync(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        // POST: Film/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Film film = await db.Films.FindAsync(id);
            db.Films.Remove(film);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
