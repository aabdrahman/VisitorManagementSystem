using Contracts;
using Entities.Model.Helpers;

namespace Repository;

public sealed class ReportFilterRepository : RepositoryBase<ReportFilterDetails>, IReportFilterDetailsRepository
{
    public ReportFilterRepository(RepositoryContext context) : base(context)
    {
    }

    public async Task<IQueryable<ReportFilterDetails>> GetReportFilterDetails(string command, params object[] prameters)
    {
        return await ExecuteProcedure(command, prameters);
    }
}
