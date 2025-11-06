using AutoMapper;
using Contracts;
using Entities.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts;

namespace Services;

public class ServiceManager : IServiceManager
{
    private readonly ILoggerManager _loggerManager;
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly Lazy<IAuthenticationService> _authenticationService;
    private readonly Lazy<IVisitDetailService> _visitDetailService;
    private readonly Lazy<IVistorService> _vistorService;
    private readonly Lazy<IReportAnalyticsService> _reportAnalyticsService;

    public ServiceManager(ILoggerManager loggerManager,
                            IRepositoryManager repositoryManager,
                            IMapper mapper, UserManager<User> userManager,
                            RoleManager<Role> roleManager,
                            IConfiguration configuration, IHttpContextAccessor contextAccessor)
    {
        _loggerManager = loggerManager;
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _contextAccessor = contextAccessor;
        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(mapper, loggerManager, repositoryManager, userManager, roleManager, configuration, _contextAccessor));
        _vistorService = new Lazy<IVistorService>(() => new VisitorService(loggerManager, repositoryManager, mapper));
        _visitDetailService = new Lazy<IVisitDetailService>(() => new VisitDetailService(repositoryManager, mapper, loggerManager, _contextAccessor));
        _reportAnalyticsService = new Lazy<IReportAnalyticsService>(() => new ReportAnalyticsService(mapper, repositoryManager, loggerManager, contextAccessor));
    }

    public IVisitDetailService VisitDetailService => _visitDetailService.Value;

    public IVistorService VistorService => _vistorService.Value;

    public IAuthenticationService AuthenticationService => _authenticationService.Value;

    public IReportAnalyticsService ReportAnalyticsService => _reportAnalyticsService.Value;
}
