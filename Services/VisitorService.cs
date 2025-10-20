using AutoMapper;
using Contracts;
using Entities.Model;
using Entities.Response;
using Service.Contracts;
using Shared.DataTransferObjects;
using System.Text.Json;

namespace Services;

public class VisitorService : IVistorService
{
    private readonly ILoggerManager _loggerManager;
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public VisitorService(ILoggerManager loggerManager, IRepositoryManager repositoryManager, IMapper mapper)
    {
        _loggerManager = loggerManager;
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<Response> CreateVisitor(CreateVisitorDto newVisitor)
    {
        try
        {
            var visiorToInsert = _mapper.Map<Visitor>(newVisitor);
            visiorToInsert.Id = Guid.NewGuid();

            _repositoryManager.VisitorRepository.Create(visiorToInsert);
            try
            {
                await _repositoryManager.SaveChanges();
            }
            catch
            {
                _loggerManager.LogError($"Error Creating Record in database");
                throw;
            }


            var createdVisitor = _mapper.Map<VisitorDto>(visiorToInsert);

            return Response.CreateSuccessResponse(createdVisitor, "Visit Creation Successful");
        }
        catch(Exception ex)
        {
            _loggerManager.LogError($"An Error Occurred!!: {ex.Message}");
            throw;
        }
        
    }

    public async Task DeleteVisitor(VisitorDto visitorToDelete)
    {
        throw new NotImplementedException();
    }

    public async Task<Response> GetAll(bool trackChanges, bool ignoreQueryFilter)
    {
        var visitorsFromDb = await _repositoryManager.VisitorRepository.GetAllVisitors(trackChanges, ignoreQueryFilter);

        var visitorToReturn = _mapper.Map<List<VisitorDto>>(visitorsFromDb);

        return visitorToReturn.Count > 0 ?
                    Response.CreateSuccessResponse(visitorToReturn, "Visitors Fetched Successfully.") :
                    Response.CreateErrorResponse(new Entities.ErrorModels.ErrorDetails { StatusCode = 99, ErrorDescription = "", Message = "No Record Found" }, "No Record Found", "99");
    }

    public async Task<Response> GetVisitor(string phoneNumber, bool trackChanges, bool ignoreQueryFilter)
    {
        try
        {
            //var visitorFromDb = _repositoryManager.VisitorRepository.GetVisitorByPhoneNumber(phoneNumber, trackChanges, ignoreQueryFilter);

            var visitorFromDb = await CheckVisitorExists(phoneNumber, trackChanges, ignoreQueryFilter);

            if (visitorFromDb == null)
            {
                return Response.CreateErrorResponse(errorDetails: new Entities.ErrorModels.ErrorDetails { StatusCode = 200, ErrorDescription = "No Record Found", Message = $"No Visitor Found with Phone Number: {phoneNumber}" }, $"No Record Found.", "99");
            }

            var visitorToReturn = _mapper.Map<VisitorDto>(visitorFromDb);

            return Response.CreateSuccessResponse(visitorToReturn, "Visitor Fetched Successfully,");
        }
        catch (Exception ex)
        {
            _loggerManager.LogError(ex.Message);
            throw;
        }
        
    }

    public async Task<Response> UpdateVisitor(VisitorDto updatedVisitor)
    {
        try
        {
            _loggerManager.LogInfo($"Updating Record for: {JsonSerializer.Serialize(updatedVisitor)}");
            var existingVisitor = await CheckVisitorExists(updatedVisitor.PhoneNumber, true, false);

            if(existingVisitor is null)
            {
                _loggerManager.LogWarning($"No visitor exists with phone number: {updatedVisitor.PhoneNumber}");
            }
            _loggerManager.LogInfo($"Updatng existing records........");
            existingVisitor.VisitorPhoneNumber = updatedVisitor.PhoneNumber;
            existingVisitor.VisitorName = updatedVisitor.VisitorName;
            existingVisitor.Status = "active";
            existingVisitor.VisitorEmailAddress = updatedVisitor?.EmailAddress ?? "";

            _repositoryManager.VisitorRepository.UpdateVisitor(existingVisitor);

            await _repositoryManager.SaveChanges();
            _loggerManager.LogInfo($"Visitor record updated successfully: {JsonSerializer.Serialize(updatedVisitor)}");
            var visitorToReturn = _mapper.Map<VisitorDto>(existingVisitor);

            return Response.CreateSuccessResponse(visitorToReturn, "Visitor Updated Successfully.");
        }
        catch (Exception ex)
        {
            _loggerManager.LogError(ex.Message);
            return Response.CreateErrorResponse(new Entities.ErrorModels.ErrorDetails() { ErrorDescription = $"{ex.InnerException?.Message}", Message = ex.Message, StatusCode = 200 }, "An Error Occurred", "99");
        }
    }

    private async Task<Visitor?> CheckVisitorExists(string phoneNumber, bool trackChanges, bool ignoreQueryFilter)
    {
        var visitorFromDb = await _repositoryManager.VisitorRepository.GetVisitorByPhoneNumber(phoneNumber, trackChanges, ignoreQueryFilter);

        return visitorFromDb;
    }
}
