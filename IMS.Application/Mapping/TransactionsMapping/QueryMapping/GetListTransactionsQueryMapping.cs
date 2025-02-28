﻿using IMS.Application.DTOs.TransactionsDTO;
using IMS.Domain.Entities;

namespace IMS.Application.Mapping.TransactionsMapping
{
    public partial class TransactionsProfile
    {
        public void GetListTransactionsQueryMapping()
        {
            CreateMap<Transaction, TransactionListDTO>();
        }
    }
}
