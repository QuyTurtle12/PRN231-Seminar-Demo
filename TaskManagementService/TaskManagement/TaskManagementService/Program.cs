using TaskManagementServiceDAO;
using TaskManagementServiceDAO.Interfaces;
using TaskManagementServiceRepo.Commands.Tasks;
using TaskManagementServiceRepo.Queries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register MediatR with all relevant assemblies
var assemblies = new[]
{
    typeof(Program).Assembly,
    typeof(GetAllTaskQuery).Assembly,
    typeof(CreateTaskCommand).Assembly
};

// Register MediatR with proper configuration
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(assemblies);
});

// Register services with the dependency injection container
builder.Services.AddScoped<TaskManagementContext>();
builder.Services.AddScoped<ITaskDAO, TaskDAO>();

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
