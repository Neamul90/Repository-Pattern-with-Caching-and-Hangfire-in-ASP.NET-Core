using Caching_Hangfire.Context;
using Caching_Hangfire.Helper;
using Caching_Hangfire.Interface;
using Caching_Hangfire.Repository;
using Caching_Hangfire.Services;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.
                       Configuration.GetConnectionString("DefaultConnection"),
                       b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
builder.Services.Configure<CacheConfiguration>(builder.Configuration.GetSection("CacheConfiguration"));
builder.Services.AddMemoryCache();
builder.Services.AddTransient<MemoryCacheService>();
builder.Services.AddTransient<RedisCacheService>();
builder.Services.AddTransient<Func<CacheTech, ICacheService>>(serviceProvider => key =>
{
    switch (key)
    {
        case CacheTech.Memory:
            return serviceProvider.GetService<MemoryCacheService>();
        case CacheTech.Redis:
            return serviceProvider.GetService<RedisCacheService>();
        default:
            return serviceProvider.GetService<MemoryCacheService>();
    }
});
builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();
#region Repositories
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHangfireDashboard("/jobs");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
