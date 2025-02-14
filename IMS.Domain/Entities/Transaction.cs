using IMS.Domain.Entities.Identities;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public int QuantityInStock { get; set; }
        public string TotalAmount { get; set; }
        public DateTime Date { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("Transaction")]
        public virtual Product Product { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("ListOfTransactions")]
        public virtual User User { get; set; }
        public virtual Payment Payment { get; set; }
    }
}
