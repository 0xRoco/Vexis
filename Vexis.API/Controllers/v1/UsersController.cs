using System.ComponentModel.DataAnnotations;
using badLogg.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vexis.API.Data;
using Vexis.Database;
using Vexis.Security;

namespace Vexis.API.Controllers.v1;

[Route("api/v1/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private LogManager Logger { get; }
    private DatabaseService Database { get; }

    public UsersController(LogManager logger, DatabaseService database)
    {
        Logger = logger;
        Database = database;
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> Get(string username, [Required] [FromHeader] string loginToken)
    {
        try
        {
            var account = await Database.AccountsManager.GetAccountAsync(username);
            if (account == null!) return StatusCode(404, new ApiResponse("Account not found", false, null));
            if (account.LoginToken != loginToken)
                return StatusCode(401, new ApiResponse("Invalid login token", false, null));

            var user = new UserModel
            {
                Username = account.Username,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                ProfilePictureUrl = account.ProfilePictureUrl,
                IsEmailVerified = account.IsEmailVerified,
                IsPhoneNumberVerified = account.IsPhoneNumberVerified
            };

            return StatusCode(200, new ApiResponse("User found", true, user));
        }
        catch (Exception e)
        {
            Logger.Error($"Error while getting user {username} : {e.Message}");
            return StatusCode(500, new ApiResponse("Error while getting user", false, null));
        }
    }

    [HttpPut("{username}")]
    public async Task<IActionResult> Put(string username, [FromBody] UserModel userModel,
        [FromHeader] [Required] string loginToken)
    {
        try
        {
            var account = await Database.AccountsManager.GetAccountAsync(username);
            if (account == null!) return StatusCode(404, new ApiResponse("Account not found", false, null));
            if (account.LoginToken != loginToken)
                return StatusCode(401, new ApiResponse("Invalid login token", false, null));

            account.Username = userModel.Username;
            account.Email = userModel.Email;
            account.PhoneNumber = userModel.PhoneNumber;
            account.ProfilePictureUrl = userModel.ProfilePictureUrl;
            account.IsEmailVerified = userModel.IsEmailVerified;
            account.IsPhoneNumberVerified = userModel.IsPhoneNumberVerified;
            account.UpdatedAt = DateTime.Now;
            var result = await Database.AccountsManager.UpdateAccountAsync(account.Id, account);

            var u = new UserModel
            {
                Username = result.Username,
                Email = result.Email,
                PhoneNumber = result.PhoneNumber,
                ProfilePictureUrl = result.ProfilePictureUrl,
                IsEmailVerified = result.IsEmailVerified,
                IsPhoneNumberVerified = result.IsPhoneNumberVerified
            };

            return StatusCode(200, new ApiResponse("User updated", true, u));
        }
        catch (Exception e)
        {
            Logger.Error($"Error updating user {username}: {e.Message}");
            return StatusCode(500, new ApiResponse("Error updating user", false, null));
        }
    }

    [AllowAnonymous]
    [Route("Register")]
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        try
        {
            Logger.Info($"Registering new user: {model.Username}");
            var account = await Database.AccountsManager.GetAccountAsync(model.Username);
            if (account != null!)
            {
                Logger.Warn($"User {model.Username} already exists");
                return StatusCode(400, new ApiResponse("Account already exists", false, null));
            }

            account = new AccountModel
            {
                Id = await SecurityService.GenerateId(),
                Username = model.Username,
                Email = model.Email,
                PasswordHash = await SecurityService.GetHash(model.Password),
                PhoneNumber = "-",
                EmailCode = await SecurityService.GenerateCode(),
                PhoneNumberCode = await SecurityService.GenerateCode(),
                LoginToken = await SecurityService.GenerateToken(),
                PasswordResetToken = "-",
                ProfilePictureUrl = "-",
                IsEmailVerified = false,
                IsPhoneNumberVerified = false,
                IsDeleted = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                LastLoginAt = default,
                LastLoginIp = "-",
                LastLoginHwid = "-"
            };
            var result = await Database.AccountsManager.CreateAccountAsync(account);
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (result == null)
            {
                Logger.Warn($"Failed to create account: {model.Username} due to database error");
                return StatusCode(StatusCodes.Status418ImATeapot,
                    new ApiResponse("Error creating account", false, null));
            }

            Logger.Info($"Account created: {model.Username} ({account.Id})");
            return StatusCode(StatusCodes.Status200OK, new ApiResponse("Account created", true, account));
        }
        catch (Exception e)
        {
            Logger.Error($"Error occured while creating account: {model.Username} ({model.Email}) - {e.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponse("Error creating account", false, null));
        }
    }

    [AllowAnonymous]
    [Route("Auth")]
    [HttpPost]
    public async Task<IActionResult> Auth([FromBody] AuthModel model)
    {
        try
        {
            AccountModel accountModel;
            if (model.UsernameOrEmail.Contains('@'))
                accountModel = await Database.AccountsManager.GetAccountAsync(model.UsernameOrEmail, true);
            else
                accountModel = await Database.AccountsManager.GetAccountAsync(model.UsernameOrEmail);

            if (accountModel == null! || !await SecurityService.VerifyHash(model.Password, accountModel.PasswordHash))
            {
                Logger.Warn(
                    $"Failed to authenticate user: {model.UsernameOrEmail} due to invalid credentials or account not found");
                return StatusCode(404, new ApiResponse("Account not found or Invalid credentials", false, null));
            }

            var loginToken = await SecurityService.GenerateToken();
            accountModel.LoginToken = loginToken;
            accountModel.UpdatedAt = DateTime.Now;
            accountModel.LastLoginAt = DateTime.Now;
            await Database.AccountsManager.UpdateAccountAsync(accountModel.Id, accountModel);
            Logger.Info($"User {accountModel.Username} ({accountModel.Id}) authenticated");
            return StatusCode(200, new ApiResponse("Login successful", true, loginToken));
        }
        catch (Exception e)
        {
            Logger.Error($"Error occured while authenticating user: {model.UsernameOrEmail} - {e.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiResponse("Error authenticating user", false, null));
        }
    }
}