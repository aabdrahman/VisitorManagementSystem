using Contracts;

namespace Repository;

public class UserSummaryDetailsRepository : RepositoryBase<Entities.Model.Helpers.UserSummaryDetails>, IUserSummaryDetails
{
    public UserSummaryDetailsRepository(RepositoryContext context) : base(context)
    {
    }

    public async Task<IQueryable<Entities.Model.Helpers.UserSummaryDetails>> GetUsers(string command, params object[] args)
    {
        return await ExecuteProcedure(command, args);
    }
}
