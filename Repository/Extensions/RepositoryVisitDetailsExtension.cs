using Entities.Model;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions;

public static class RepositoryVisitDetailsExtension
{
    public static IQueryable<VisitDetail> FilterByDate(this IQueryable<VisitDetail> entities, VisitDetailRequestParameter visitDetailRequestParameter)
    {
        return entities.Where(x => x.VisitationDate >= visitDetailRequestParameter.startDate && x.VisitationDate <= visitDetailRequestParameter.endDate);
    }

    public static IQueryable<VisitDetail> SearchByStatus(this IQueryable<VisitDetail> visitDetails, VisitDetailRequestParameter visitDetailRequestParameter)
    {
        if (visitDetailRequestParameter.Status is null)
            return visitDetails;

        var filterStatus = visitDetailRequestParameter.Status;

        return visitDetails.Where(x => x.VisitStatus == filterStatus);

    }
}
