using Microsoft.EntityFrameworkCore;
using Vexis.API.Data;

namespace Vexis.Database.Contexts;

public class AccountContext : DbContext
{
    public DbSet<AccountModel> Accounts { get; set; }
    private string ConnectionString { get; }

    public AccountContext(string connectionString)
    {
        ConnectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySQL(ConnectionString);
    }
}