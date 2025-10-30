using Entities.Model;
using Shared.RequestFeatures;

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

    public static IQueryable<VisitDetail> SearchByHostName(this IQueryable<VisitDetail> visitDetails, VisitDetailRequestParameter visitDetailRequestParameter)
    {
        if(string.IsNullOrEmpty(visitDetailRequestParameter.HostName))
            return visitDetails;

        return visitDetails.Where(x => x.HostName.Contains(visitDetailRequestParameter.HostName.Replace(".", " ")));
    }
}
