using Contracts;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<IVisitorRepository> _visitorRepository;
    private readonly Lazy<IVisitDetailRepository> _visitDetailRepository;
    private readonly Lazy<IUserSummaryDetails> _userSummaryDetails;
    private readonly Lazy<IReportFilterDetailsRepository> _reportFilterDetailsRepository;
    private readonly RepositoryContext _context;

    public RepositoryManager(RepositoryContext repositoryContext)
    {
        _context = repositoryContext;
        _visitDetailRepository = new Lazy<IVisitDetailRepository>(() => new VisitDetailRepository(repositoryContext));
        _visitorRepository = new Lazy<IVisitorRepository>(() => new VisitorRepository(repositoryContext));
        _userSummaryDetails = new Lazy<IUserSummaryDetails>(() => new UserSummaryDetailsRepository(repositoryContext));
        _reportFilterDetailsRepository = new Lazy<IReportFilterDetailsRepository>(() => new ReportFilterRepository(repositoryContext));
    }

    public IVisitDetailRepository VisitDetailRepository => _visitDetailRepository.Value;

    public IVisitorRepository VisitorRepository => _visitorRepository.Value;

    public IUserSummaryDetails UserSummaryDetails => _userSummaryDetails.Value;

    public IReportFilterDetailsRepository ReportFilterDetailsRepository => _reportFilterDetailsRepository.Value;

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}
