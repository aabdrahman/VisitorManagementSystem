using Entities.Model;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class VisitorRepository : RepositoryBase<Visitor>, IVisitorRepository
{
    public VisitorRepository(RepositoryContext context) : base(context)
    {
    }

    public void AdVisitor(Visitor visitor)
    {
        Create(visitor);
    }

    public void Delete(Visitor visitor)
    {
        Delete(visitor);
    }

    public async Task<IEnumerable<Visitor>> GetAllVisitors(bool trackChanges, bool ignoreQueryFilter)
    {
        return await FindAll(trackChanges, ignoreQueryFilter).OrderBy(v => v.VisitorName).ToListAsync();
    }

    public async Task<Visitor?> GetById(Guid Id, bool trackChanges, bool ignoreQueryFilter)
    {
        return await FindByCondition(x => x.Id == Id, trackChanges, ignoreQueryFilter).SingleOrDefaultAsync()!;
    }

    public async Task<Visitor> GetVisitorByPhoneNumber(string phoneNumber, bool trackChanges, bool ignoreQueryFilter)
    {
        return await FindByCondition(v => v.VisitorPhoneNumber == phoneNumber, trackChanges, ignoreQueryFilter).SingleOrDefaultAsync()!;
    }

    public void UpdateVisitor(Visitor visitor)
    {
        Update(visitor);
    }
}
