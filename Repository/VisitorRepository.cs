using Entities.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public async Task<Visitor> GetVisitorByPhoneNumber(string phoneNumber, bool trackChanges, bool ignoreQueryFilter)
    {
        return await FindByCondition(v => v.VisitorPhoneNumber == phoneNumber, trackChanges, ignoreQueryFilter).SingleOrDefaultAsync()!;
    }


}
