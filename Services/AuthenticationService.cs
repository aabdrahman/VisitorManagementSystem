using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using Shared.DataTransferObjects;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly ILoggerManager _loggerManager;
    private readonly IRepositoryManager _repositoryManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private User? loginUser;

    public AuthenticationService(IMapper mapper, 
                                ILoggerManager loggerManager, 
                                IRepositoryManager repositoryManager, 
                                UserManager<User> userManager, 
                                RoleManager<Role> roleManager, 
                                IConfiguration configuration)
    {
        _repositoryManager = repositoryManager;
        _loggerManager = loggerManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<IdentityResult> CreateRole(RoleForRegistrationDto roleForRegistration, string token)
    {
        _loggerManager.LogInfo($"Creating Role: {roleForRegistration}");
        var roleToInsert = _mapper.Map<Role>(roleForRegistration);
        roleToInsert.CreatedBy = GetPrincipalsFromToken(token).Identity.Name!;

        var result = await _roleManager.CreateAsync(roleToInsert);

        if(!result.Succeeded)
        {
            _loggerManager.LogError($"An Error Occurred creating role in the: {nameof(CreateRole)} method. ");
            throw new RoleCreateFailureException("An Error Occurred creating Role.");
        }
        _loggerManager.LogInfo($"Role Creation Successful: {roleToInsert}");
        return result;
    }

    public async Task<IdentityResult> CreateUser(UserForCreationDto userForCreation)
    {
        var existsRoles = await CheckRolesExist(userForCreation);

        if (!existsRoles.isValidRoles)
            return IdentityResult.Failed(new IdentityError { Description = $"Provided roles: {string.Concat(existsRoles.notExistingRoles)} does not exist", Code = "99" });
        
        var userToInsert = _mapper.Map<User>(userForCreation);

        var result = await _userManager.CreateAsync(userToInsert, userForCreation.Password);

        if(result.Succeeded)
            await _userManager.AddToRolesAsync(userToInsert, userForCreation.UserRoles);

        //await _userManager.AddToRolesAsync(userToInsert, userForCreation.UserRoles);

        return result;
    }

    public async Task<TokenDto> GenerateToken(bool populateExp)
    {
        var claims = await GetClaims();
        var signingCredentials = GetCredentials();
        var tokenGenerator = GetTokenOptions(claims, signingCredentials);
        var refreshToken = GenerateRefreshToken();

        loginUser.RefreshToken = refreshToken;


        if (populateExp)
            loginUser.TokenExpirationDate = DateTime.Now.AddMinutes(7);

        await _userManager.UpdateAsync(loginUser);

        var token = new JwtSecurityTokenHandler().WriteToken(tokenGenerator);
        return new TokenDto(AccessToken: token, refreshToken: refreshToken);
    }

    public async Task<IEnumerable<RoleDto>> GetRoles()
    {
        var result = _roleManager.Roles.ToList();

        var rolesToReturn = _mapper.Map<List<RoleDto>>(result);

        return rolesToReturn;
    }

    public async Task<bool> ValidateUser(UserToLoginDto userToLogin)
    {
        loginUser = await _userManager.FindByNameAsync(userToLogin.UserName);

        //var confirmPasswordCorrect = await _userManager.CheckPasswordAsync(user!, userToLogin.Password);

        var result = loginUser is not null && await _userManager.CheckPasswordAsync(loginUser, userToLogin.Password);

        if (!result)
            _loggerManager.LogWarning($"{nameof(ValidateUser)}: Authentication Failed For user. Invalid Username or Password");

        return result;

    }

    public async Task<string> ResetPassword(ChangePasswordDto resetPasswordDetails)
    {
        var user = await _userManager.FindByNameAsync(resetPasswordDetails.UserName);

        if (user is null)
        {
            _loggerManager.LogError($"{nameof(resetPasswordDetails.UserName)}: Authentication Failed for user. Invalid User name.");
            return string.Empty;
        }

        _loggerManager.LogInfo($"Removing password for user: {resetPasswordDetails.UserName}");
        var result = await _userManager.RemovePasswordAsync(user);

        if(!result.Succeeded)
        {
            _loggerManager.LogError($"User - {resetPasswordDetails.UserName}: Password Remove Unsuccessful. Error: {string.Join(", ",result.Errors.Select(x => x.Description))}");
            return "Error removing user password.";
        }

        var updateResult = await _userManager.AddPasswordAsync(user, resetPasswordDetails.Password);

        if(!updateResult.Succeeded)
        {
            _loggerManager.LogError($"User Name: {resetPasswordDetails.UserName} - Error: {string.Join(", ", updateResult.Errors.Select(x => x.Description))}");
        }

        return "Passwrd Update Successful";
    }

    public async Task<TokenDto> GenerateRefreshToken(TokenDto tokenDto)
    {
        var principal = GetPrincipalsFromToken(tokenDto.AccessToken);

        var user = await _userManager.FindByNameAsync(principal.Identity.Name);

        if (user is null || user.TokenExpirationDate < DateTime.Now || user.RefreshToken != tokenDto.refreshToken)
            throw new BadTokenException();
        loginUser = user;
        var token = await GenerateToken(populateExp: false);

        return token;
    }

    public async Task<string> GetLoginUser(string token)
    {
        var principals = GetPrincipalsFromToken(token);

        var user = await _userManager.FindByNameAsync(principals.Identity.Name);

        return $"{user.FirstName} {user.LastName}";
    }

    private ClaimsPrincipal GetPrincipalsFromToken(string token)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = _configuration["JwtSecretKey"];

        var tokenOptions = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidAudience = jwtSettings["ValidAudience"],
            ValidIssuer = jwtSettings["ValidIssuer"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;

        var principal = tokenHandler.ValidateToken(token, tokenOptions, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if(jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid Token");
        }

        return principal;
    }

    private JwtSecurityToken GetTokenOptions(List<Claim> claims, SigningCredentials signingCredentials)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var tokenOptions = new JwtSecurityToken
        (
            issuer: jwtSettings["ValidIssuer"],
            audience: jwtSettings["ValidAudience"],
            signingCredentials: signingCredentials,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["Duration"]))
        );

        return tokenOptions;
    }

    private string GenerateRefreshToken()
    {
       // var refreshToken = string.Empty;

        var rndNum = new byte[32];

        using(var randNum = RandomNumberGenerator.Create())
        {
            randNum.GetBytes(rndNum);
            //refreshToken = Convert.ToBase64String(rndNum);
        }

        return Convert.ToBase64String(rndNum); ;
    }

    private SigningCredentials GetCredentials()
    {
        var key = _configuration["JwtSecretKey"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        return credentials;
    }

    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, loginUser?.UserName!),
            new Claim(ClaimTypes.Email, loginUser?.Email!),
            new Claim(ClaimTypes.NameIdentifier, loginUser?.StaffId!),
            new Claim(ClaimTypes.SerialNumber, loginUser?.Id?.ToString()!),
            new Claim("FirstName", loginUser?.FirstName!),
            new Claim("LastName", loginUser?.LastName!)
        };
        var roles = await _userManager.GetRolesAsync(loginUser!);

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        return claims;

    }

    private async Task<(bool isValidRoles, ICollection<string> notExistingRoles)> CheckRolesExist(UserForCreationDto userForCreation)
    {
        var userDefinedRoles = userForCreation.UserRoles;
        ICollection<string> notExistingRoles = [];

        var isValid = true;

        if(userDefinedRoles == null)
            isValid = false;

        foreach (var role in userDefinedRoles)
        {
            if(!await _roleManager.RoleExistsAsync(role))
            {
                notExistingRoles.Add(role);
            }
        }

        isValid = notExistingRoles.Count > 0 ? false : true;

        return (isValid, notExistingRoles);
    }

}
