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
    public class CategoryController : Controller
    {
        private FilmCatalogueContext db = new FilmCatalogueContext();

        // GET: Category
        public async Task<ActionResult> Index()
        {
            return View(await db.Categories.ToListAsync());
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            var categories = db.Categories.ToList();
            categories.Insert(0, new Category { Id = 0, Name = "None" });
            ViewBag.ParentCategoryId = new SelectList(categories, "Id", "Name");
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,ParentCategoryId")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.ParentCategoryId == 0)
                {
                    category.ParentCategoryId = null;
                }
                db.Categories.Add(category);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            var categories = db.Categories.Where(c => c.Id != id).ToList();
            categories.Insert(0, new Category { Id = 0, Name = "None" });
            ViewBag.ParentCategoryId = new SelectList(categories, "Id", "Name", category.ParentCategoryId);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,ParentCategoryId")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.ParentCategoryId == 0)
                {
                    category.ParentCategoryId = null;
                }
                db.Entry(category).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Category/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            db.Categories.Remove(category);
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
