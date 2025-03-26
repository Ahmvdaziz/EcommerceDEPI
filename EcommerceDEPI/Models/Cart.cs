using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Cart
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();
    public int Quantity { get; set; }
}
