using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Model;
using Entities.Response;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    private async Task<Visitor?> CheckVisitorExists(string phoneNumber, bool trackChanges, bool ignoreQueryFilter)
    {
        var visitorFromDb = await _repositoryManager.VisitorRepository.GetVisitorByPhoneNumber(phoneNumber, trackChanges, ignoreQueryFilter);

        return visitorFromDb;
    }
}
