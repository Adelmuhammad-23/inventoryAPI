using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public string Supplier { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [InverseProperty("Products")]
        public virtual Category Category { get; set; }
        public virtual LowStockAlert LowStockAlert { get; set; }
        public virtual Transaction Transaction { get; set; }


    }
}
