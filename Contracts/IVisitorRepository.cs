using Entities.Model;

namespace Repository;

public interface IVisitorRepository
{
    Task<IEnumerable<Visitor>> GetAllVisitors(bool trackChanges, bool ignoreQueryFilter);
    Task<Visitor> GetVisitorByPhoneNumber(string phoneNumber, bool trackChanges, bool ignoreQueryFilter);
    Task<Visitor> GetById(Guid Id, bool trackChanges, bool ignoreQueryFilter);
    void Create(Visitor visitor);
    void Delete(Visitor visitor);
    void AdVisitor(Visitor visitor);
    void UpdateVisitor(Visitor visitor);
}
