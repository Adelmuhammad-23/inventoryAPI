using AutoMapper;
using IMS.Application.DTOs.TransactionsDTO;
using IMS.Domain.Entities;
using IMS.Domain.Enums;
using IMS.Domain.Interfaces;
using IMS.Domain.UnitOfWorkInterface;
using IMS.Infrastructure.ExternalServices;


namespace IMS.Application.Services
{
    public class TransactionsServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILowStockAlerts _lowStockAlerts;
        private readonly EmailService _emailService;
        private readonly IMapper _mapper;

        public TransactionsServices(EmailService emailService, IUnitOfWork unitOfWork, ITransactionsRepository transactionsRepository, IProductRepository productRepository, ILowStockAlerts lowStockAlerts, IMapper mapper)
        {
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _transactionsRepository = transactionsRepository;
            _productRepository = productRepository;
            _lowStockAlerts = lowStockAlerts;
            _mapper = mapper;
        }

        public async Task<TransactiontDTO> GetTransactionsByIdAsync(int id)
        {
            var transaction = await _unitOfWork.TransactionUOF.GetByIdAsync(id);

            if (transaction is null)
                return null;
            var transactionsMapping = _mapper.Map<TransactiontDTO>(transaction);

            return transactionsMapping;
        }
        public async Task<IEnumerable<TransactionListDTO>> GetSaleTransactionsAsync()
        {
            var transactionsList = await _transactionsRepository.GetSaleTransactionsAsync();
            if (transactionsList is null)
                return null;
            var transactionsMapping = _mapper.Map<IEnumerable<TransactionListDTO>>(transactionsList);

            return transactionsMapping;
        }
        public async Task<IEnumerable<TransactionListDTO>> GetPurchaseTransactionsAsync()
        {
            var transactionsList = await _transactionsRepository.GetPurchaseTransactionsAsync();
            if (transactionsList is null)
                return null;
            var transactionsMapping = _mapper.Map<IEnumerable<TransactionListDTO>>(transactionsList);

            return transactionsMapping;
        }
        public async Task<string> AddTransactionsAsync(AddTransactionDTO model)
        {
            var product = await _unitOfWork.ProductsUOF.GetByIdAsync(model.ProductId);
            var transactionMapping = _mapper.Map<Transaction>(model);

            if (model.Type.ToLower() == TransactionTypeEnum.Sale.ToString().ToLower())
            {
                if (product.QuantityInStock < model.Quantity)
                    return "Not enough stock available.";

                product.QuantityInStock -= model.Quantity;

                var alert = await _lowStockAlerts.GetByIdAsync(product.Id);
                if (alert != null && product.QuantityInStock < alert.Threshold && !alert.AlertSent)
                {
                    await _emailService.SendEmailAsync("adelmuhammad.r@gmail.com",
                                                        $"Low Stock Alert: {product.Name}",
                                                        $"⚠️ Warning: The stock for {product.Name} has dropped to {product.QuantityInStock}. Please restock soon!"); // ✉️ إرسال الإيميل
                    alert.AlertSent = true;
                    await _lowStockAlerts.UpdatAsync(alert);
                }
            }
            else if (model.Type.ToLower() == TransactionTypeEnum.Purchase.ToString().ToLower())
                product.QuantityInStock += model.Quantity;
            else
                return "Invalid transaction type. Use 'sale' or 'purchase'.";


            var transactionsResult = await _unitOfWork.TransactionUOF.AddAsync(transactionMapping);
            await _unitOfWork.Complete();
            if (transactionsResult is null)
                return "Error when add Transaction !.";

            return "Success";
        }

    }
}
