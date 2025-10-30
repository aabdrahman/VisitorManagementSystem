using Entities.Model;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared.RequestFeatures;

namespace Repository;

public class VisitDetailRepository : RepositoryBase<VisitDetail>, IVisitDetailRepository
{
    public VisitDetailRepository(RepositoryContext context) : base(context)
    {
    }

    public async Task<VisitDetail?> ConfirmCardNumberAvailable(string cardNumber)
    {
        var existingCheckedInRecord = await FindByCondition(x => x.AssignedCardNumber == cardNumber && x.VisitStatus == Entities.StaticValues.VisitStatus.CheckedIn, trackChanges: false, ignoreQueryFilter: true).FirstOrDefaultAsync();

        return existingCheckedInRecord;
    }

    public void CreateVisitetail(VisitDetail visitDetail)
    {
        Create(visitDetail);
    }

    public void Delete(VisitDetail visitDetail)
    {
        Delete(visitDetail);
    }

    public async Task<PagedList<VisitDetail>> GetAll(VisitDetailRequestParameter visitDetailRequestParameter, bool trackChanges, bool hasQueryFilter)
    {
        var visitDetails = await FindAll(trackChanges, hasQueryFilter)
                        .FilterByDate(visitDetailRequestParameter)
                        .SearchByStatus(visitDetailRequestParameter)
                        .SearchByHostName(visitDetailRequestParameter)
                        .OrderByDescending(x => x.CreatedDate)
                        //.Skip((visitDetailRequestParameter.pageNumber - 1) * visitDetailRequestParameter.PageSize)
                        //.Take(visitDetailRequestParameter.PageSize)
                        .ToListAsync();

        //var pagedListVisitDetails = new PagedList<VisitDetail>(visitDetails, visitDetails.Count, visitDetailRequestParameter.pageNumber, visitDetailRequestParameter.PageSize);

        var pagedListVisitDetails = PagedList<VisitDetail>.ToPagedList(visitDetails, visitDetails.Count, visitDetailRequestParameter.PageSize, visitDetailRequestParameter.pageNumber);

        return pagedListVisitDetails;
    }

    public async Task<VisitDetail> GetByIdentificationNumber(string identificationNumber, bool tranckChanges, bool hasQueryFilter)
    {
        return await FindByCondition(v => v.VisitorIdentificationNumber == identificationNumber, tranckChanges, hasQueryFilter).SingleOrDefaultAsync()!;
    }

    public void UpdateRecord(VisitDetail visitDetail)
    {
        Update(visitDetail);
    }
}
