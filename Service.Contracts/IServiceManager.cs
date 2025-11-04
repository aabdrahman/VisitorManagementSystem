namespace Service.Contracts;

public interface IServiceManager
{
    IVisitDetailService VisitDetailService { get; }
    IVistorService VistorService { get; }
    IAuthenticationService AuthenticationService { get; }
    IReportAnalyticsService ReportAnalyticsService { get; }
}
