using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts;

public interface IServiceManager
{
    IVisitDetailService VisitDetailService { get; }
    IVistorService VistorService { get; }
    IAuthenticationService AuthenticationService { get; }
}
