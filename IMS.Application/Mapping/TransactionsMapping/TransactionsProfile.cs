using AutoMapper;

namespace IMS.Application.Mapping.TransactionsMapping
{
    public partial class TransactionsProfile : Profile
    {
        public TransactionsProfile()
        {
            GetListTransactionsQueryMapping();
            GetTransactionByIdQueryMapping();
            AddTrasactionCommandMapping();

        }
    }
}
