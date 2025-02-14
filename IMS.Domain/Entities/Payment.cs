using IMS.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public string Threshold { get; set; }
        public bool PaymentMethod { get; set; }
        public int Amount { get; set; }
        public PaymentStatusEnum PaymentStatus { get; set; }
        public int TransactionId { get; set; }
        [ForeignKey("TransactionId")]
        [InverseProperty("Payment")]
        public virtual Transaction Transaction { get; set; }
    }
}
