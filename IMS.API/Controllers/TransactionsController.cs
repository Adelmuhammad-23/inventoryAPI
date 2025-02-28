using IMS.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionsServices _transactionsServices;

        public TransactionsController(TransactionsServices transactionsServices)
        {
            _transactionsServices = transactionsServices;
        }

        [HttpGet("Sale")]
        public async Task<IActionResult> GetAllSaleTransactions()
        {
            var transactions = await _transactionsServices.GetSaleTransactionsAsync();
            if (transactions == null)
                return NotFound("Not found transactions");
            return Ok(transactions);
        }
        [HttpGet("Purchase")]
        public async Task<IActionResult> GetAllPurchaseTransactions()
        {
            var transactions = await _transactionsServices.GetPurchaseTransactionsAsync();
            if (transactions == null)
                return NotFound("Not found transactions");
            return Ok(transactions);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllPurchaseTransactions([FromRoute] int id)
        {
            var transactions = await _transactionsServices.GetTransactionsByIdAsync(id);
            if (transactions == null)
                return NotFound($"Not found transaction with ID:{id}");
            return Ok(transactions);
        }
    }
}
