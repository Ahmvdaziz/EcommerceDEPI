using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using EcommerceDEPI.Models;
using Microsoft.IdentityModel.Tokens;

namespace EcommerceDEPI.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

       public IActionResult Index()
{
    var products = _db.Products.Include(p => p.Category).ToList();
    return View(products); // تمرير IEnumerable<Product> إلى View
}


public IActionResult Details(int id)
{
    var product = _db.Products
        .Include(p => p.Category)
        .FirstOrDefault(p => p.Id == id);

    if (product == null)
        return NotFound();

    return View(product);
}

        [HttpGet]
        public IActionResult Create()
        {
            var cats = _db.Categories.Select(a => a.Name).ToList();
            ViewData["cats"] = cats;
            return View(new Product());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Product pData, string category)
        {
            if (string.IsNullOrEmpty(pData.Name))
            {
                ModelState.AddModelError("Name", "Name Can't be empty");
                return View(pData);
            }


            if (string.IsNullOrEmpty(pData.Description))
            {
                ModelState.AddModelError("Description", "Description Can't be empty");
                return View(pData);
            }

            if (pData.Amount < 0)
            {
                ModelState.AddModelError("Amount", "The Product's amount can't be less than 0");
                return View(pData);
            }

            if (pData.Price < 0)
            {
                ModelState.AddModelError("Price", "The Product's Price can't be less than 0");
                return View(pData);
            }

            var oldP = await _db.Products.Where(a => a.Name == pData.Name).FirstOrDefaultAsync();
            if (oldP != null)
            {
                ModelState.AddModelError("Name", "There is already a Product with this name");
                return View(pData);
            }

            var cat = await _db.Categories.Where(c => c.Name == category).FirstOrDefaultAsync();

            if (cat == null)
            {
                return View(pData);
            }

            pData.CategoryId = cat.Id;
            pData.Category = cat;
            pData.Picture = "";

            _db.Products.Add(pData);
            await _db.SaveChangesAsync();
            return RedirectToAction("ViewP");
        }



        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int pID)
        {
            var oldP = await _db.Products.Where(a => a.Id == pID).FirstOrDefaultAsync();
            if (oldP == null)
            {
                return RedirectToAction("ViewP");
            }
            _db.Products.Remove(oldP);
            await _db.SaveChangesAsync();
            return RedirectToAction("ViewP");

        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View(new Product());
        }

        [HttpPost]
        public IActionResult Edit(Product cData)
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ViewP()
        {
            var pros = await _db.Products.ToListAsync();
            ViewData["pros"] = pros;
            return View();
        }
    }
}
