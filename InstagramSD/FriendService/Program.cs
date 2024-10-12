using Common.ServiceBus;
using Common.Settings;
using FriendService.Data;
using FriendService.Repositories;
using FriendService.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//services
builder.Services.AddScoped<IFriendService, FriendService.Services.FriendService>();
builder.Services.AddScoped<IFollowRepository, FollowRepository>();
builder.Services.AddScoped<IEventBus, EventBus>();

//settings
builder.Services.Configure<ServiceBusSettings>(builder.Configuration.GetSection("ServiceBusSettings"));

//database
var CosmosDbEndpoint = "AddCosmosDbEndpoint";
var CosmosDbAuthKey = "addCosmosDbAuthKey";
var CosmosDbName = "instagramdb";

builder.Services.AddDbContext<FriendDbContext>(
   options => options.UseCosmos(
       CosmosDbEndpoint,
       CosmosDbAuthKey,
       CosmosDbName,
       cosmosCLientOptions =>
           new CosmosClientOptions
           {
               IdleTcpConnectionTimeout = new System.TimeSpan(1, 0, 0, 0)
           }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

