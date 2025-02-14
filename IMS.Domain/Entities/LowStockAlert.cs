using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Domain.Entities
{
    public class LowStockAlert
    {
        public int Id { get; set; }
        public string Threshold { get; set; }
        public bool AlertSent { get; set; }
        public DateTime Date { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("LowStockAlert")]
        public virtual Product Product { get; set; }
    }
}
