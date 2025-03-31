using Microsoft.AspNetCore.Mvc;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using EcommerceDEPI.Models;


namespace EcommerceDEPI.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Category());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Category cData)
        {
            if (string.IsNullOrEmpty(cData.Name))
            {
                ModelState.AddModelError("Name", "Name Can't be empty");
                return View(cData);
            }

            var oldC = await _db.Categories.Where(a => a.Name == cData.Name).FirstOrDefaultAsync();
            if (oldC != null)
            {
                ModelState.AddModelError("Name", "There is already a category with this name");
                return View(cData);
            }
            
            _db.Categories.Add(cData);
            await _db.SaveChangesAsync();
            return RedirectToAction("ViewC");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int cID)
        {
            var oldC = await _db.Categories.Where(a => a.Id == cID).FirstOrDefaultAsync();
            if (oldC == null)
            {
                return RedirectToAction("ViewC");
            }
            _db.Categories.Remove(oldC);
            await _db.SaveChangesAsync();
            return RedirectToAction("ViewC");

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int cID)
        {
            var oldC = await _db.Categories.Where(a => a.Id == cID).FirstOrDefaultAsync();
            if (oldC == null)
            {
                return RedirectToAction("ViewC");
            }
            return View(oldC);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(Category cData)
        {
            var existingCategory = await _db.Categories.FindAsync(cData.Id);

            var oldC = await _db.Categories.Where(a => a.Name == cData.Name).FirstOrDefaultAsync();
            if (oldC == null) // new name and no one got it before
            {
                // do nothing for now
            }
            else if (cData.Id != oldC.Id) // new name but already obtained by another row
            {
                ModelState.AddModelError("Name", "There is already a category with this name");

                return View(cData);
            }
            // else same name no change


            existingCategory.Name = cData.Name;
            _db.Categories.Update(existingCategory);
            await _db.SaveChangesAsync();

            return RedirectToAction("ViewC");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ViewC()
        {
            var cats = await _db.Categories.ToListAsync();
            ViewData["cats"] = cats;
            return View();
        }
    }
}
