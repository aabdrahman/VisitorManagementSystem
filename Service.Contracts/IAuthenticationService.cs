using Entities.Model.Helpers;
using Microsoft.AspNetCore.Identity;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service.Contracts;

public interface IAuthenticationService
{
    Task<IdentityResult> CreateRole(RoleForRegistrationDto roleForRegistration, string token);
    Task<IEnumerable<RoleDto>> GetRoles();
    Task<IdentityResult> CreateUser(UserForCreationDto userForCreation);
    Task<bool> ValidateUser(UserToLoginDto userToLogin);
    Task<TokenDto> GenerateToken(bool populateExp);
    Task<string> GetLoginUser(string token);
    Task<TokenDto> GenerateRefreshToken(TokenDto tokenDto);
    Task<string> ResetPassword(ChangePasswordDto resetPasswordDetails);
    Task<IEnumerable<UserSummaryDetails>> GetAllUsers(UsersRequestParameter usersRequestParameter);
}
