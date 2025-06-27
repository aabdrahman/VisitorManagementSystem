using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts;

public interface IVisitDetailService
{
    Task<VisitDetailDto> GetVisitDetailsByIdentificationNumber(string visitorIdentificationNumber, bool trackChanges, bool ignoreQueryFilter);
    Task<VisitDetailDto> ScheduleVisit(ScheduleVisitDetailDto scheduledVisit);
    Task<(IEnumerable<VisitDetailDto> visits, MetaData metaData)> GetAllVisits(VisitDetailRequestParameter visitDetailRequestParameter, bool trackChanges, bool ignoreQueryFilter);
    Task<SuccessfulCheckInDetailsDto> UpdateCheckIn(VisitorDetailsCheckInDto checkInDetails);
    Task<SuccessfulCheckInDetailsDto> UpdateCheckOut(VisitorDetailsCheckInDto visitorDetailsCheckIn);

}
