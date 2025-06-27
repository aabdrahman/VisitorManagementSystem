using Entities.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public interface IVisitorRepository
{
    Task<IEnumerable<Visitor>> GetAllVisitors(bool trackChanges, bool ignoreQueryFilter);
    Task<Visitor> GetVisitorByPhoneNumber(string phoneNumber, bool trackChanges, bool ignoreQueryFilter);
    void Create(Visitor visitor);
    void Delete(Visitor visitor);
    void AdVisitor(Visitor visitor);    
}
