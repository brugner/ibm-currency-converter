using AutoMapper;
using IBM.Application.Models.DTOs.Transactions;
using IBM.Domain.Entities;

namespace IBM.Application.MappingProfiles;

public class TransactionMappingProfile : Profile
{
    public TransactionMappingProfile()
    {
        CreateMap<ExternalTransactionDTO, Transaction>();
        CreateMap<Transaction, TransactionDTO>();
        CreateMap<TransactionDTO, SimpleTransactionDTO>();
    }
}