using Common.ServiceBus;
using Common.Settings;
using LikeService.Data;
using LikeService.Repositories;
using LikeService.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//service
builder.Services.AddScoped<ILikeService, LikeService.Services.LikeService>();
builder.Services.AddScoped<IEventBus, EventBus>();

//repositories
builder.Services.AddScoped<ILikeRepository, LikeRepository>();

//settings
builder.Services.Configure<ServiceBusSettings>(builder.Configuration.GetSection("ServiceBusSettings"));

//dbConfig

var CosmosDbEndpoint = "AddCosmosDbEndpoint";
var CosmosDbAuthKey = "addCosmosDbAuthKey";
var CosmosDbName = "instagramdb";

builder.Services.AddDbContext<LikeDbContext>(
   options => options.UseCosmos(
       CosmosDbEndpoint,
       CosmosDbAuthKey,
       CosmosDbName,
       cosmosCLientOptions =>
           new CosmosClientOptions
           {
               IdleTcpConnectionTimeout = new System.TimeSpan(1, 0, 0, 0)
           }));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();

    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options =>
{
    options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

