using IMS.Application.DTOs.TransactionsDTO;
using IMS.Application.Services;

using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionsServices _transactionsServices;
        private readonly ProductsServices _productsServices;

        public TransactionsController(TransactionsServices transactionsServices, ProductsServices productsServices)
        {
            _transactionsServices = transactionsServices;
            _productsServices = productsServices;

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
        [HttpPost("record-transaction")]
        public async Task<IActionResult> RecordTransaction([FromBody] AddTransactionDTO model)
        {
            var findProduct = await _productsServices.GetProductAsync(model.ProductId);
            if (findProduct == null)
                return NotFound($"Not found product with thiw id: {model.ProductId}");

            var transactions = await _transactionsServices.AddTransactionsAsync(model);
            if (transactions == null)
                return BadRequest($"Not found transaction to added");
            return Ok("This transaction has been done ");

        }

    }
}
