
using NineDotAssessment.Application;
using NineDotAssessment.Infrastructure;
using NineDotAssessment.Infrastructure.Persistence;
using NineDotAssessment.Middlewares;
using Serilog.Events;
using Serilog;
using System.Diagnostics;
using Serilog.Exceptions;

var builder = WebApplication.CreateBuilder(args);

ConfigureActivity();
ConfigureLogger(builder.Environment.EnvironmentName);
builder.Host.UseSerilog();
// Add services to the container.
builder.Services.ConfigureInfrastructureService(builder.Configuration);
builder.Services.AddApplicationService();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();


app.UseHttpsRedirection();

app.UseAuthorization();


// migrate any database changes on startup (includes initial db creation)
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dataContext.Database.EnsureCreated();
}


app.MapControllers();

app.Run();




void ConfigureLogger(string? environment)
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .Enrich.WithProperty("NineDot", "Test-API")
        .Enrich.WithProperty("application", "test-api")
        .Enrich.WithProperty("environment", environment)
        .WriteTo.Console()
        .WriteTo.File($"Logs/log-{DateTime.Now.Date}.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();
}

void ConfigureActivity()
{
    Activity.DefaultIdFormat = ActivityIdFormat.W3C;
    var listener = new ActivityListener
    {
        ShouldListenTo = _ => true,
        ActivityStopped = activity =>
        {
            foreach (var (key, value) in activity.Baggage)
            {
                activity.AddTag(key, value);
            }
        }
    };
    ActivitySource.AddActivityListener(listener);
}
