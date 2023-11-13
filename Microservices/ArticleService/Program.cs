using Microsoft.EntityFrameworkCore;
using ArticleService.Data;
using ArticleService.EventProcessing;
using ArticleService.SyncDataServices.Grpc;
using JwtAuthenticationManager;
using ArticleService.AsyncDataServices;
using RedisCaching.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCustomJwtAuthentication();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("cnn"));
});

builder.Services.AddScoped<IResponseCacheService, ResponseCacheService>(p =>
    new ResponseCacheService(builder.Configuration.GetValue<bool>("RedisConfiguration:Enabled"), builder.Configuration.GetValue<string>("RedisConfiguration:ConnectionString")));

builder.Services.AddScoped<IArticleRepo, ArticleRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHostedService<MessageBusSubscriber>();
builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddScoped<ICategoryDataClient, CategoryDataClient>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
PrepDb.PrepPopulation(app);
app.Run();
