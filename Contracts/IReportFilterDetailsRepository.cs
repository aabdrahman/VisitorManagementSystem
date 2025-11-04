using Entities.Model.Helpers;

namespace Contracts;

public interface IReportFilterDetailsRepository
{
    Task<IQueryable<ReportFilterDetails>> GetReportFilterDetails(string command, params object[] prameters);
}
