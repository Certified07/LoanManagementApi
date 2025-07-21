using LoanManagementApi.Implementations.Repositories;
using LoanManagementApi.Implementations.Services;
using LoanManagementApi.Interfaces.Repositories;
using LoanManagementApi.Interfaces.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MyContext>(options =>
   options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

//add repositories
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<IRepaymentRepository, RepaymentRepository>();
builder.Services.AddScoped<IRepaymentScheduleRepository, RepaymentScheduleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILoanDurationRuleRepository, LoanDurationRuleRepository>();

//add services
builder.Services.AddScoped<ILoanDurationRuleService, LoanDurationRuleService>();
builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddScoped<IRepaymentService, RepaymentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();  // This was missing!
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();