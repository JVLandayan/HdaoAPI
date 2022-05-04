using Hangfire;
using Hangfire.MemoryStorage;
using HumanDaoAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var services = builder.Services;

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddScoped<IChainService, ChainService>();
services.AddScoped<ICMarketCapService, CMarketCapService>();

services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();
//IRecurringJobManager recurringJobManager = 


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//recurringJobManager.AddOrUpdate("Run every day",
//    () => app.Services.GetService<IBackgroundService>().PostStoryBlok(), Cron.Minutely);

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.UseCors("EnableCORS");

app.Run();
