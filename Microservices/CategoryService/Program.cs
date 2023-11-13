using CategoryService.AsyncDataServices;
using CategoryService.Data;
using CategoryService.SyncDataServices.Grpc;
using Microsoft.EntityFrameworkCore;
using JwtAuthenticationManager;
using CategoryService.Configurations;
using RedisCaching.Services;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddCustomJwtAuthentication();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("cnn"));
});
var redisConfiguration = new RedisConfiguration();
builder.Configuration.GetSection("RedisConfiguration").Bind(redisConfiguration);
builder.Services.AddSingleton(redisConfiguration);
builder.Services.AddScoped<IResponseCacheService, ResponseCacheService>(p=> new ResponseCacheService(redisConfiguration.Enabled, redisConfiguration.ConnectionString));

builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddGrpc();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGrpcService<GrpcCategoryService>();

app.Run();
