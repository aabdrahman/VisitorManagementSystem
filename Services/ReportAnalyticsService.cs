using AutoMapper;
using Contracts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;
using Shared.DataTransferObjects.Report;
using System.Text.Json;

namespace Services;

public sealed class ReportAnalyticsService : IReportAnalyticsService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    public ReportAnalyticsService(IMapper mapper, IRepositoryManager repositoryManager, ILoggerManager loggerManager)
    {
        _mapper = mapper;
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
    }
    public async Task<ReportAnalyticsSummaryDto> GetAnalyticsReport(ReportAnalyticsBoundaryDto reportAnalyticsBoundary)
    {
        _loggerManager.LogInfo($"Getting Analytics with Parameters: {JsonSerializer.Serialize(reportAnalyticsBoundary)}");
        SqlParameter startDateParameter = new SqlParameter("@StartDate", reportAnalyticsBoundary.StartDate);
        SqlParameter endDateParamter = new SqlParameter("@EndDate", reportAnalyticsBoundary.EndDate);
        _loggerManager.LogInfo($"Executing Report Analytics Procedures for parameters: {JsonSerializer.Serialize(reportAnalyticsBoundary)}");
        var reportByVisitStatusAsQueryable = await _repositoryManager.ReportFilterDetailsRepository.GetReportFilterDetails(@"EXECUTE [dbo].[ProcGetByVisitStatusReport] 
                           @StartDate
                          ,@EndDate", startDateParameter, endDateParamter);

        var reportByVisitTypeAsQueryable = await _repositoryManager.ReportFilterDetailsRepository.GetReportFilterDetails(@"EXECUTE [dbo].[ProcGetByVisitTypeReport] 
                           @StartDate
                          ,@EndDate", startDateParameter, endDateParamter);

        var reportByVisitRegistrationTypeAsQueryable = await _repositoryManager.ReportFilterDetailsRepository.GetReportFilterDetails(@"EXECUTE [dbo].[ProcGetByVisitRegistrationTypeReport]  
                           @StartDate
                          ,@EndDate", startDateParameter, endDateParamter);

        var reportByVisitRegistrationType = _mapper.Map<List<ReportFilterDto>>(await reportByVisitRegistrationTypeAsQueryable.ToListAsync());
        var reportByVisitType = _mapper.Map<List<ReportFilterDto>>(await reportByVisitTypeAsQueryable.ToListAsync());
        var reportByVisitStatus = _mapper.Map<List<ReportFilterDto>>(await reportByVisitStatusAsQueryable.ToListAsync());

        var reportAnalyticsSummary = new ReportAnalyticsSummaryDto(reportByVisitStatus, reportByVisitRegistrationType, reportByVisitType);
        _loggerManager.LogInfo($"Report Analytics fetched: {JsonSerializer.Serialize(reportAnalyticsSummary)}");

        return reportAnalyticsSummary;  
    }
}
