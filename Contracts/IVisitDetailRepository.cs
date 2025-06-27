using Entities.Model;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public interface IVisitDetailRepository
{
    void UpdateRecord(VisitDetail visitDetail);
    void Delete(VisitDetail visitDetail);
    Task<PagedList<VisitDetail>> GetAll(VisitDetailRequestParameter visitDetailRequestParameter, bool trackChanges, bool hasQueryFilter);
    Task<VisitDetail> GetByIdentificationNumber(string identificationNumber, bool tranckChanges, bool hasQueryFilter);
    void CreateVisitetail(VisitDetail visitDetail);
    Task<VisitDetail?> ConfirmCardNumberAvailable(string cardNumber);
}
