using System.Reflection;
using badLogg.Core;
using Vexis.Database;

var builder = WebApplication.CreateBuilder(args);

// Add LogManager
var logger = new LogManager(new LogConfig()
    .SetAppName("Vexis.API")
    .SetLogDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Vexis\\Vexis.API\\Logs")
    .SetMaxLogs(3)
    .WithConsoleLogging()
    .WithFileLogging()
);

logger.Info($"Logging started at {DateTime.Now}");
logger.Info($"App root: {Environment.CurrentDirectory}");
logger.Info($"Log directory: {logger.GetConfig().LogDirectory}");
logger.Info($"{logger.GetConfig().AppName} v{Assembly.GetExecutingAssembly().GetName().Version}");


// Create database context
var connectionString = builder.Configuration["Vexis:devDB"];
var dbService = new DatabaseService(connectionString, logger);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton(logger);
builder.Services.AddSingleton(dbService);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();