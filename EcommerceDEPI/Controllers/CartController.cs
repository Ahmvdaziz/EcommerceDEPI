using Microsoft.AspNetCore.Mvc;
using EcommerceApp.Models;
namespace EcommerceDEPI.Models
{

    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, string userId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);

            if (product != null)
            {
                if (cart == null)
                {
                    // لو المستخدم ليس لديه سلة، يتم إنشاؤها
                    cart = new Cart { UserId = userId };
                    _context.Carts.Add(cart);
                    _context.SaveChanges();
                }

                // التأكد من أن المنتج غير مكرر في السلة
                var cartProduct = _context.CartProducts.FirstOrDefault(cp => cp.CartId == cart.Id && cp.ProductId == product.Id);

                if (cartProduct == null)
                {
                    cartProduct = new CartProduct
                    {
                        CartId = cart.Id,
                        ProductId = product.Id
                    };
                    _context.CartProducts.Add(cartProduct);
                }

                cart.Quantity++; // تحديث العدد الكلي للعناصر بالسلة
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Product");
        }
    }
}