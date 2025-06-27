using AutoMapper;
using Contracts;
using Entities.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services;

public class ServiceManager : IServiceManager
{
    private readonly ILoggerManager _loggerManager;
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly Lazy<IAuthenticationService> _authenticationService;
    private readonly Lazy<IVisitDetailService> _visitDetailService;
    private readonly Lazy<IVistorService> _vistorService;

    public ServiceManager(ILoggerManager loggerManager, 
                            IRepositoryManager repositoryManager, 
                            IMapper mapper, UserManager<User> userManager,
                            RoleManager<Role> roleManager, 
                            IConfiguration configuration)
    {
        _loggerManager = loggerManager;
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(mapper, loggerManager, repositoryManager, userManager, roleManager, configuration));
        _vistorService = new Lazy<IVistorService>(() => new VisitorService(loggerManager, repositoryManager, mapper));
        _visitDetailService = new Lazy<IVisitDetailService>(() => new VisitDetailService(repositoryManager, mapper, loggerManager));
    }

    public IVisitDetailService VisitDetailService => _visitDetailService.Value;

    public IVistorService VistorService => _vistorService.Value;

    public IAuthenticationService AuthenticationService => _authenticationService.Value;
}
