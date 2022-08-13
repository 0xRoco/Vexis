using badLogg.Core;
using Microsoft.EntityFrameworkCore;
using Vexis.API.Data;
using Vexis.Database.Contexts;

namespace Vexis.Database;

public class AccountsManager
{
    private string ConnectionString { get; }

    private LogManager Logger { get; }

    public AccountsManager(LogManager logger, string connectionString)
    {
        ConnectionString = connectionString;
        Logger = logger;
    }


    public async Task<List<AccountModel>> GetAccountsAsync()
    {
        await using var db = new AccountContext(ConnectionString);
        try
        {
            return await db.Accounts.ToListAsync();
        }
        catch (Exception e)
        {
            Logger.Error($"Error getting accounts list: {e.GetBaseException()}");
            return null!;
        }
        finally
        {
            await db.DisposeAsync();
        }
    }

    public async Task<AccountModel> GetAccountAsync(int id)
    {
        await using var db = new AccountContext(ConnectionString);
        try
        {
            var result = await db.Accounts.FindAsync(id);
            if (result != null) return result;

            Logger.Warn($"Failed to find account with id {id}");
            return null!;
        }
        catch (Exception e)
        {
            Logger.Error($"Error getting account with id {id}: {e.GetBaseException()}");
            return null!;
        }
        finally
        {
            await db.DisposeAsync();
        }
    }

    public async Task<AccountModel> GetAccountAsync(string usernameOrEmail, bool isEmail = false)
    {
        await using var db = new AccountContext(ConnectionString);
        try
        {
            if (isEmail)
            {
                var result = await db.Accounts.FirstOrDefaultAsync(a => a.Email == usernameOrEmail);
                if (result != null) return result;
            }
            else
            {
                var result = await db.Accounts.FirstOrDefaultAsync(a => a.Username == usernameOrEmail);
                if (result != null) return result;
            }

            Logger.Warn($"Failed to find account with username/email {usernameOrEmail}");
            return null!;
        }
        catch (Exception e)
        {
            Logger.Error($"Error getting account with username/email {usernameOrEmail}: {e.GetBaseException()}");
            return null!;
        }
        finally
        {
            await db.DisposeAsync();
        }
    }

    public async Task<bool> CreateAccountAsync(AccountModel accountModel)
    {
        await using var db = new AccountContext(ConnectionString);
        try
        {
            if (await AccountExistsAsync(accountModel)) return false;
            await db.Accounts.AddAsync(accountModel);
            if (await db.SaveChangesAsync() > 0)
            {
                Logger.Info($"Account {accountModel.Username} ({accountModel.Id}) created");
                return true;
            }

            Logger.Warn($"Failed to create account {accountModel.Username} ({accountModel.Id})");
            return false;
        }
        catch (Exception e)
        {
            Logger.Error($"Error creating account {accountModel.Username} ({accountModel.Id}): {e.GetBaseException()}");
            return false;
        }
        finally
        {
            await db.DisposeAsync();
        }
    }

    public async Task<AccountModel> UpdateAccountAsync(int id, AccountModel accountModel)
    {
        await using var db = new AccountContext(ConnectionString);
        try
        {
            var targetAccount = await db.Accounts.FindAsync(id);
            if (targetAccount == null) return null!;
            db.Accounts.Remove(targetAccount);
            await Task.Delay(500);
            await db.Accounts.AddAsync(accountModel);
            if (await db.SaveChangesAsync() > 0)
            {
                Logger.Info($"Account {accountModel.Username} ({accountModel.Id}) updated");
                return targetAccount;
            }
            else
            {
                Logger.Warn($"Failed to update account {accountModel.Username} ({accountModel.Id})");
                return null!;
            }
        }
        catch (Exception e)
        {
            Logger.Error($"Error updating account {accountModel.Username} ({id}): {e.GetBaseException()}");
            return null!;
        }
        finally
        {
            await db.DisposeAsync();
        }
    }

    public async Task<bool> DeleteAccountAsync(int id)
    {
        await using var db = new AccountContext(ConnectionString);
        try
        {
            var targetAccount = await db.Accounts.FindAsync(id);
            if (targetAccount == null) return false;
            db.Accounts.Remove(targetAccount);
            if (await db.SaveChangesAsync() > 0)
            {
                Logger.Info($"Account {targetAccount.Username} ({targetAccount.Id}) deleted");
                return true;
            }

            Logger.Warn($"Failed to delete account {targetAccount.Username} ({targetAccount.Id})");
            return false;
        }
        catch (Exception e)
        {
            Logger.Error($"Error deleting account {id}: {e.GetBaseException()}");
            return false;
        }
        finally
        {
            await db.DisposeAsync();
        }
    }

    public async Task<bool> AccountExistsAsync(AccountModel accountModel)
    {
        await using var db = new AccountContext(ConnectionString);
        try
        {
            return await db.Accounts.AnyAsync(x => x.Id == accountModel.Id) ||
                   await db.Accounts.AnyAsync(x => x.Username == accountModel.Username) ||
                   await db.Accounts.AnyAsync(x => x == accountModel);
        }
        catch (Exception e)
        {
            Logger.Error($"Error checking account {accountModel.Username} ({accountModel.Id}): {e.GetBaseException()}");
            return false;
        }
        finally
        {
            await db.DisposeAsync();
        }
    }
}