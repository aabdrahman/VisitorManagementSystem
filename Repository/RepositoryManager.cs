using Contracts;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<IVisitorRepository> _visitorRepository;
    private readonly Lazy<IVisitDetailRepository> _visitDetailRepository;
    private readonly Lazy<IUserSummaryDetails> _userSummaryDetails;
    private readonly RepositoryContext _context;

    public RepositoryManager(RepositoryContext repositoryContext)
    {
        _context = repositoryContext;
        _visitDetailRepository = new Lazy<IVisitDetailRepository>(() => new VisitDetailRepository(repositoryContext));
        _visitorRepository = new Lazy<IVisitorRepository>(() => new VisitorRepository(repositoryContext));
        _userSummaryDetails = new Lazy<IUserSummaryDetails>(() => new UserSummaryDetailsRepository(repositoryContext));
    }

    public IVisitDetailRepository VisitDetailRepository => _visitDetailRepository.Value;

    public IVisitorRepository VisitorRepository => _visitorRepository.Value;

    public IUserSummaryDetails UserSummaryDetails => _userSummaryDetails.Value;

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}
