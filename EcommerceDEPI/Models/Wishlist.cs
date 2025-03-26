using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Wishlist
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<WishlistProduct> WishlistProducts { get; set; } = new List<WishlistProduct>();


}
