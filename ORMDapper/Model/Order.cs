using Dapper.Contrib.Extensions;

namespace ORMDapper.Model;

[Table("[Order]")]
public class Order
{
    [Key]
    public int OrderId { get; set; }
    public string Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public int ProductId { get; set; }
}