#define MOCK
using Serilog;
using TaskManagmentApp.Business.Interfaces;
using TaskManagmentApp.Business.Managers;
using TaskManagmentApp.DataAccess.DataProviders;
using TaskManagmentApp.DataAccess.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Get configuration
var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();


// Add services to the container.
builder.Services.AddControllers();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#if MOCK
//Mocks should be deleted or replaced later
builder.Services.AddSingleton<ITaskDataProvider, MockTaskDataProvider>();
builder.Services.AddSingleton<IUserDataProvider, MockUserDataProvider>();
#else
//Mocks should be deleted or replaced later
builder.Services.AddScoped<ITaskDataProvider, MockTaskDataProvider>();
builder.Services.AddScoped<IUserDataProvider, MockUserDataProvider>();
#endif

builder.Services.AddScoped<ITaskManager, TaskManager>();
builder.Services.AddScoped<IUserManager, UserManager>();


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
