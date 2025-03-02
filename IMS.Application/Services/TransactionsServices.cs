using AutoMapper;
using IMS.Application.DTOs.TransactionsDTO;
using IMS.Domain.Entities;
using IMS.Domain.Enums;
using IMS.Domain.Interfaces;
using IMS.Domain.UnitOfWorkInterface;


namespace IMS.Application.Services
{
    public class TransactionsServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public TransactionsServices(IUnitOfWork unitOfWork, ITransactionsRepository transactionsRepository, IProductRepository productRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _transactionsRepository = transactionsRepository;
            _productRepository = productRepository;

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
        public async Task<AddTransactionDTO> AddTransactionsAsync(AddTransactionDTO model)
        {
            var product = await _unitOfWork.ProductsUOF.GetByIdAsync(model.ProductId);
            var transactionMapping = _mapper.Map<Transaction>(model);

            if (model.Type == TransactionTypeEnum.Sale.ToString())
                product.QuantityInStock -= model.Quantity;
            else if (model.Type == TransactionTypeEnum.Purchase.ToString())
                product.QuantityInStock += model.Quantity;

            var transactionsResult = await _unitOfWork.TransactionUOF.AddAsync(transactionMapping);
            await _unitOfWork.Complete();
            if (transactionsResult is null)
                return null;

            return model;
        }

    }
}
