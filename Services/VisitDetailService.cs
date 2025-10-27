using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Model;
using Entities.Response;
using Entities.StaticValues;
using Microsoft.AspNetCore.Http;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Text.Json;

namespace Services;

public class VisitDetailService : IVisitDetailService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _loggerManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public VisitDetailService(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager loggerManager, IHttpContextAccessor httpContextAccessor)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _loggerManager = loggerManager;
        _httpContextAccessor = httpContextAccessor;
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

    public async Task<Response> CreateVisit(CreateVisitDetailDto createVisitDetail)
    {
        _loggerManager.LogInfo($"Creating record for: {JsonSerializer.Serialize(createVisitDetail)}");
        var visitDetail = _mapper.Map<VisitDetail>(createVisitDetail);
        visitDetail.VisitStatus = VisitStatus.Pending;
        //visitDetail.VisitationDate = DateOnly.FromDateTime(DateTime.UtcNow);
        //visitDetail.VisitType = Entities.StaticValues.VisitType.WalkIn;
        //visitDetail.VisitorRegistrationType = Entities.StaticValues.VisitorRegistrationTypes.FirstTime;
        //visitDetail.CreatedDate = DateTime.UtcNow;
        visitDetail.VisitorIdentificationNumber = GenerateVisitorIdentificationNumber();

        if (visitDetail.VisitorRegistrationType == VisitorRegistrationTypes.FirstTime)
        {
            var visitor = new CreateVisitorDto(VisitorName: createVisitDetail.VisitorName, PhoneNumber: createVisitDetail.VisitorPhoneNumber, EmailAdddress: createVisitDetail.VisitorEmailAddress, createVisitDetail.VisitorGender);

            var visitorToInsert = _mapper.Map<Visitor>(visitor);

            _repositoryManager.VisitorRepository.Create(visitorToInsert);
        }

        _repositoryManager.VisitDetailRepository.CreateVisitetail(visitDetail);

        await _repositoryManager.SaveChanges();
        _loggerManager.LogInfo($"Visit detail Created. Visitor Identification Number: {visitDetail.VisitorIdentificationNumber}");
        var visitDetailToReturn = _mapper.Map<VisitDetailDto>(visitDetail);
        return Response.CreateSuccessResponse(visitDetailToReturn, $"Visit Created. Identification Number: {visitDetail.VisitorIdentificationNumber}");
    }

    public async Task<Response> UpdateStatus(UpdateVisitStatusDto updateVisitDetailStatus)
    {
        _loggerManager.LogInfo($"Updating Visit Status for: {JsonSerializer.Serialize(updateVisitDetailStatus)}");
        var existingVisitDetail = await CheckVisitExists(updateVisitDetailStatus.VisitorIdentificationNumber, true, false);

        if(existingVisitDetail is null)
        {
            _loggerManager.LogWarning($"No visit detail for Visitor Identification Number: {updateVisitDetailStatus.VisitorIdentificationNumber}");
            return Response.CreateErrorResponse(null, $"No Visit for provided Id: {updateVisitDetailStatus.VisitorIdentificationNumber}", "99");
        }

        if(existingVisitDetail.VisitStatus == VisitStatus.CheckedOut)
        {
            _loggerManager.LogWarning($"Provided Visit Detail has an invalid status: {existingVisitDetail.VisitStatus}");
            return Response.CreateErrorResponse(null, $"Invalid Id provided: Status: {existingVisitDetail.VisitStatus.ToString()}", "90");
        }

        existingVisitDetail.VisitStatus = updateVisitDetailStatus.UpdatedStatus;

        _repositoryManager.VisitDetailRepository.UpdateRecord(existingVisitDetail);
        _loggerManager.LogInfo($"Updating Visit Status for: {updateVisitDetailStatus.VisitorIdentificationNumber} to {updateVisitDetailStatus.UpdatedStatus.ToString()}");
        await _repositoryManager.SaveChanges();

        return Response.CreateSuccessResponse("", "Updated Successfully");
    }

    public async Task<VisitDetailDto> ScheduleVisit(ScheduleVisitDetailDto scheduledVisit)
    {
        _loggerManager.LogInfo($"Schedule Visit For: {JsonSerializer.Serialize(scheduledVisit)}");
        var visitDetailToInsert = _mapper.Map<VisitDetail>(scheduledVisit);

        var visitorIdentificationNumber = GenerateVisitorIdentificationNumber();

        visitDetailToInsert.VisitorIdentificationNumber = visitorIdentificationNumber;
        visitDetailToInsert.VisitStatus = VisitStatus.Scheduled;
        visitDetailToInsert.VisitType = VisitType.Appointment;



        if(scheduledVisit.VisitorRegistrationType == VisitorRegistrationTypes.FirstTime.ToString())
        {
            _loggerManager.LogInfo($"Validate Visitor exists for: {scheduledVisit.VisitorPhoneNumber}");

            var possibleExistingVisitor = await _repositoryManager.VisitorRepository.GetVisitorByPhoneNumber(scheduledVisit.VisitorPhoneNumber, false, false);

            if(possibleExistingVisitor is null)
            {
                _loggerManager.LogInfo($"Creating Entity For first timer...");

                var visitor = new CreateVisitorDto(scheduledVisit.VisitorName, scheduledVisit.VisitorPhoneNumber, scheduledVisit.VisitorEmailAddress, scheduledVisit.VisitorGender);

                var visitorToInsert = _mapper.Map<Visitor>(visitor);
                _repositoryManager.VisitorRepository.AdVisitor(visitorToInsert);
                visitDetailToInsert.VisitorRegistrationType = VisitorRegistrationTypes.Recurring;
                _loggerManager.LogInfo($"Visitor Inserted. Pending Database Update.");
            }
           
        }

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
        if(visitDetails.VisitStatus != VisitStatus.Scheduled && visitDetails.VisitStatus != VisitStatus.Pending)
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
        if (visitDetails.VisitStatus != VisitStatus.CheckedIn)
            throw new InvalidVisitStatusException($"Visit Status is: {visitDetails.VisitStatus.ToString()}. Cannot CheckOut");

        visitDetails.CheckOutTime = DateTime.Now;
        visitDetails.VisitStatus = VisitStatus.CheckedOut;

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
