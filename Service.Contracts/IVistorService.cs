using Entities.Response;
using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface IVistorService
{
    Task<Response> GetAll(bool trackChanges, bool ignoreQueryFilter);
    Task<Response> GetVisitor(string phoneNumber, bool trackChanges, bool ignoreQueryFilter);
    Task<Response> CreateVisitor(CreateVisitorDto newVisitor);
    Task DeleteVisitor(VisitorDto visitorToDelete);
    Task<Response> UpdateVisitor(VisitorDto updatedVisitor);
}
