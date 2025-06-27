using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Model;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services;

public class VisitDetailService : IVisitDetailService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;

    public VisitDetailService(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager loggerManager)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _loggerManager = loggerManager;
    }

    public async Task<(IEnumerable<VisitDetailDto> visits, MetaData metaData)> GetAllVisits(VisitDetailRequestParameter visitDetailRequestParameter, bool trackChanges, bool ignoreQueryFilter)
    {
        if (!visitDetailRequestParameter.isValidDate())
            throw new InvalidFilterDateException(visitDetailRequestParameter.startDate, visitDetailRequestParameter.endDate);

        var AllVisitDetailsFromDb = await _repositoryManager.VisitDetailRepository.GetAll(visitDetailRequestParameter, trackChanges, ignoreQueryFilter);

        var allVisitsToReturn = _mapper.Map<List<VisitDetailDto>>(AllVisitDetailsFromDb);


        return (visits: allVisitsToReturn, metaData: AllVisitDetailsFromDb.metaData);
    }

    public async Task<VisitDetailDto> GetVisitDetailsByIdentificationNumber(string visitorIdentificationNumber, bool trackChanges, bool ignoreQueryFilter)
    {
        var visitDetailRecord = await CheckVisitExists(visitorIdentificationNumber, trackChanges, ignoreQueryFilter);

        if(visitDetailRecord == null)
            throw new VisitDetailNotFoundException(visitorIdentificationNumber);

        return _mapper.Map<VisitDetailDto>(visitDetailRecord);
    }

    public async Task<VisitDetailDto> ScheduleVisit(ScheduleVisitDetailDto scheduledVisit)
    {
        _loggerManager.LogInfo($"Schedule Visit For: {scheduledVisit}");
        var visitDetailToInsert = _mapper.Map<VisitDetail>(scheduledVisit);

        var visitorIdentificationNumber = GenerateVisitorIdentificationNumber();

        visitDetailToInsert.VisitorIdentificationNumber = visitorIdentificationNumber;
        visitDetailToInsert.VisitStatus = Entities.StaticValues.VisitStatus.Scheduled;
        visitDetailToInsert.VisitType = Entities.StaticValues.VisitType.Appointment;

        _loggerManager.LogInfo($"Inserting Record Into Database: {visitDetailToInsert}");
        _repositoryManager.VisitDetailRepository.CreateVisitetail(visitDetailToInsert);

        await _repositoryManager.SaveChanges();

        _loggerManager.LogInfo($"Visit Detail Scheduled: {visitDetailToInsert}");

        var createdScheduledVisit = _mapper.Map<VisitDetailDto>(visitDetailToInsert);

        return createdScheduledVisit;
    }

    public async Task<SuccessfulCheckInDetailsDto> UpdateCheckIn(VisitorDetailsCheckInDto checkInDetails)
    {
        _loggerManager.LogInfo($"Check Visit Exists For: {checkInDetails.VisitorIdentificationNumber}");
        var visitDetails = await CheckVisitExists(checkInDetails.VisitorIdentificationNumber, trackChanges: true, ignoreQueryFilter: true);

        if (visitDetails == null)
            throw new VisitDetailNotFoundException(checkInDetails.VisitorIdentificationNumber);
        if(visitDetails.VisitStatus != Entities.StaticValues.VisitStatus.Scheduled && visitDetails.VisitStatus != Entities.StaticValues.VisitStatus.Scheduled)
            throw new InvalidVisitStatusException(visitDetails.VisitStatus.ToString());
        if (!await ValidateCardNumberAvailable(checkInDetails))
            throw new InvalidCardDetailsException(checkInDetails.CardNumber);



        visitDetails.ReceptionistName = checkInDetails.ReceptionistName;
        visitDetails.CheckTime = DateTime.Now;
        visitDetails.AssignedCardNumber = checkInDetails.CardNumber;
        visitDetails.VisitStatus = Entities.StaticValues.VisitStatus.CheckedIn;

        _loggerManager.LogInfo($"Inserting Record Into Databse: {visitDetails}");

        _repositoryManager.VisitDetailRepository.UpdateRecord(visitDetails);
        await _repositoryManager.SaveChanges();
        _loggerManager.LogInfo($"Insertion Successful. Mapping to Outpute DTO: {visitDetails}");
        var visitDetailToReturn = _mapper.Map<SuccessfulCheckInDetailsDto>(visitDetails);

        return visitDetailToReturn;

    }

    public async Task<SuccessfulCheckInDetailsDto> UpdateCheckOut(VisitorDetailsCheckInDto visitorDetailsCheckIn)
    {
        var visitDetails = await CheckVisitExists(visitorDetailsCheckIn.VisitorIdentificationNumber, trackChanges: true, ignoreQueryFilter: true);

        if (visitDetails == null)
            throw new VisitDetailNotFoundException(visitorDetailsCheckIn.VisitorIdentificationNumber);
        if (visitDetails.VisitStatus != Entities.StaticValues.VisitStatus.CheckedIn)
            throw new InvalidVisitStatusException($"Visit Status is: {visitDetails.VisitStatus.ToString()}. Cannot CheckOut");

        visitDetails.CheckOutTime = DateTime.Now;
        visitDetails.VisitStatus = Entities.StaticValues.VisitStatus.CheckedOut;

        _repositoryManager.VisitDetailRepository.UpdateRecord(visitDetails);
        await _repositoryManager.SaveChanges();

        var visitDetailsToReturn = _mapper.Map<SuccessfulCheckInDetailsDto>(visitDetails);

        return visitDetailsToReturn;

    }

    private async Task<VisitDetail?> CheckVisitExists(string visitorIdentificationNumber, bool trackChanges, bool ignoreQueryFilter)
    {
        var visitDetail = await _repositoryManager.VisitDetailRepository.GetByIdentificationNumber(visitorIdentificationNumber, trackChanges, ignoreQueryFilter);

        return visitDetail;
    }

    private async Task<bool> ValidateCardNumberAvailable(VisitorDetailsCheckInDto visitorDetailsCheckIn)
    {
        var existingRecord = await _repositoryManager.VisitDetailRepository.ConfirmCardNumberAvailable(visitorDetailsCheckIn.CardNumber);

        return existingRecord is null;
    }

    private string GenerateVisitorIdentificationNumber()
    {
        var randNum = new Random();

        var genRandNum = randNum.NextInt64(1000, 999999);

        var paddedNum = genRandNum.ToString().PadLeft(6, '0');

        var visitorIdentificationNumber = string.Concat("VIS-" ,paddedNum, DateTime.Now.ToString("yyyyMMddhhmmss"));

        return visitorIdentificationNumber;
    }
}
