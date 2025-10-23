using Entities.Model.Helpers;

namespace Contracts;

public interface IUserSummaryDetails
{
    Task<IQueryable<UserSummaryDetails>> GetUsers(string command, params object[] args);
}
