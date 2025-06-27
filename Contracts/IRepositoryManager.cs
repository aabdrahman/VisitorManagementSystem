using Entities.Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts;

public interface IRepositoryManager
{
    IVisitDetailRepository VisitDetailRepository { get; }
    IVisitorRepository VisitorRepository { get; }
    Task SaveChanges();
}
