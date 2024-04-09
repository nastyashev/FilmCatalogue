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

        // GET: Film/Details/5
        public async Task<ActionResult> Details(int? id)
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

        // GET: Film/Create
        public ActionResult Create()
        {
            ViewBag.Categories = db.Categories.ToList();
            return View();
        }

        // POST: Film/Create
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Director,Release")] Film film)
        {
            if (ModelState.IsValid)
            {
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
            return View(film);
        }

        // POST: Film/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Director,Release")] Film film)
        {
            if (ModelState.IsValid)
            {
                db.Entry(film).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
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
