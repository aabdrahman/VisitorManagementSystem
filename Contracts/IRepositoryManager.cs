using Repository;

namespace Contracts;

public interface IRepositoryManager
{
    IVisitDetailRepository VisitDetailRepository { get; }
    IVisitorRepository VisitorRepository { get; }
    IUserSummaryDetails UserSummaryDetails { get; }
    IReportFilterDetailsRepository ReportFilterDetailsRepository { get; }
    Task SaveChanges();
}
