using Entities.Response;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts;

public interface IVistorService
{
    Task<Response> GetAll(bool trackChanges, bool ignoreQueryFilter);
    Task<Response> GetVisitor(string phoneNumber, bool trackChanges, bool ignoreQueryFilter);
    Task<Response> CreateVisitor(CreateVisitorDto newVisitor);
    Task DeleteVisitor(VisitorDto visitorToDelete);

}
