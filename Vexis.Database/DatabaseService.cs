using badLogg.Core;


namespace Vexis.Database;

public class DatabaseService
{
    public AccountsManager AccountsManager { get; }

    private string ConnectionString { get; }

    private LogManager Logger { get; }

    public DatabaseService(string connectionString, LogManager logger)
    {
        Logger = logger;
        ConnectionString = connectionString;

        AccountsManager = new AccountsManager(logger, ConnectionString);
    }
}