using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<IVisitorRepository> _visitorRepository;
    private readonly Lazy<IVisitDetailRepository> _visitDetailRepository;
    private readonly RepositoryContext _context;

    public RepositoryManager(RepositoryContext repositoryContext)
    {
        _context = repositoryContext;
        _visitDetailRepository = new Lazy<IVisitDetailRepository>(() => new VisitDetailRepository(repositoryContext));
        _visitorRepository = new Lazy<IVisitorRepository>(() => new VisitorRepository(repositoryContext));
    }

    public IVisitDetailRepository VisitDetailRepository => _visitDetailRepository.Value;

    public IVisitorRepository VisitorRepository => _visitorRepository.Value;

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}
