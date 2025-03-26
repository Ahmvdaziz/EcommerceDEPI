using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Payment
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Method { get; set; }
    public decimal Amount { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
}
