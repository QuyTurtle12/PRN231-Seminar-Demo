using Polly;
using Polly.Bulkhead;
using TaskManagementServiceDAO;
using TaskManagementServiceDAO.Interfaces;
using TaskManagementServiceRepo.Interfaces;
using TaskManagementServiceRepo.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register services with the dependency injection container
builder.Services.AddScoped<TaskManagementContext>();
builder.Services.AddScoped<ITaskDAO, TaskDAO>();
builder.Services.AddScoped<IUserDAO, UserDAO>();
builder.Services.AddScoped<ITaskRepo, TaskRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();

// Register the BulkheadPolicy as a singleton service
builder.Services.AddSingleton(sp =>
    TaskManagementServiceRepo.Policies.BulkheadPolicy.CreateBulkheadPolicy(
        sp.GetRequiredService<IConfiguration>()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
