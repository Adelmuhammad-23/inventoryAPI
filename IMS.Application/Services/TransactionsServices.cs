using AutoMapper;
using IMS.Application.DTOs.TransactionsDTO;
using IMS.Domain.Interfaces;

namespace IMS.Application.Services
{
    public class TransactionsServices
    {
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IMapper _mapper;

        public TransactionsServices(ITransactionsRepository transactionsRepository, IMapper mapper)
        {
            _transactionsRepository = transactionsRepository;
            _mapper = mapper;
        }

        public async Task<TransactiontDTO> GetTransactionsByIdAsync(int id)
        {
            var transaction = await _transactionsRepository.GetByIdAsync(id);
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
    }
}
