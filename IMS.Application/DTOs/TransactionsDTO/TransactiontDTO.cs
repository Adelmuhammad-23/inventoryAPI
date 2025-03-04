namespace IMS.Application.DTOs.TransactionsDTO
{
    public class TransactiontDTO
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public int QuantityInStock { get; set; }
        public int TotalAmount { get; set; }

        public DateTime Date { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
    }
}
