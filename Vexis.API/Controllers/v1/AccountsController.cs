using badLogg.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vexis.API.Data;
using Vexis.Database;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Vexis.API.Controllers.v1;

[Obsolete("This is intended to be used with AdminTools when its ready", true)]
[Authorize]
[Route("api/v1/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private LogManager Logger { get; }
    private DatabaseService Database { get; }

    public AccountsController(DatabaseService database, LogManager logger)
    {
        Database = database;
        Logger = logger;
    }

    // GET: api/<AccountsController>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var accounts = await Database.AccountsManager.GetAccountsAsync();
        return StatusCode(200, new ApiResponse("Accounts found", true, accounts));
    }

    // GET api/<AccountsController>/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var account = await Database.AccountsManager.GetAccountAsync(id);

        return account == null!
            ? StatusCode(404, new ApiResponse("Account not found", false, null))
            : StatusCode(200, new ApiResponse("Account found", true, account));
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> Get(string username)
    {
        var account = await Database.AccountsManager.GetAccountAsync(username);
        return account == null!
            ? StatusCode(404, new ApiResponse("Account not found", false, null))
            : StatusCode(200, new ApiResponse("Account found", true, account));
    }

    // POST api/<AccountsController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AccountModel accountModel)
    {
        if (accountModel.Id <= 0) return StatusCode(400, new ApiResponse("Account ID is invalid", false, null));

        var success = await Database.AccountsManager.CreateAccountAsync(accountModel);
        return success
            ? StatusCode(201, new ApiResponse("Account created", true, accountModel))
            : StatusCode(400, new ApiResponse("Account creation failed", false, null));
    }

    // PUT api/<AccountsController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] AccountModel accountModel)
    {
        if (accountModel.Id <= 0) return StatusCode(400, new ApiResponse("Account ID is invalid", false, null));

        var result = await Database.AccountsManager.UpdateAccountAsync(id, accountModel);
        var success = result != null!;
        return success
            ? StatusCode(200, new ApiResponse("Account updated", true, result))
            : StatusCode(400, new ApiResponse("Account update failed", false, null));
    }

    // DELETE api/<AccountsController>/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await Database.AccountsManager.DeleteAccountAsync(id);
        return success
            ? StatusCode(200, new ApiResponse("Account deleted", true, null))
            : StatusCode(400, new ApiResponse("Account deletion failed", false, null));
    }
}