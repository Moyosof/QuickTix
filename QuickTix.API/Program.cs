using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuickTix.API.Data;
using QuickTix.API.Entities;
using QuickTix.API.Extensions;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//.CreateDefaultBuilder(args)
//.ConfigureLogging(logging =>
//{
//    logging.AddConsole();
//    logging.AddDebug();
//});

// Add services to the container.
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureRepositoryAndServices(builder.Configuration);

builder.Services.ConfigureMapping();

builder.Services.ConfigureDatabaseConnection(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//builder.Services.AddDbContext<ExpenseTrackerDbContext>(options => options.UseSqlServer(
//    builder.Configuration.GetConnectionString("DefaultConnection")));



//builder.Services.ManageDataAsync(builder.Services);

//var dbContextSvc = .GetRequiredService<ExpenseTrackerDbContext>();








//builder.Services.AddScoped<IExpense, ExpenseRepository>();
//builder.Services.AddScoped<IUserAuthRepository, UserAuthRepository>();


//var mapperConfig = new MapperConfiguration(map =>
//{
//    map.AddProfile<ExpenseMapping>();
//    map.AddProfile<UserMapping>();

//});

//builder.Services.AddSingleton(mapperConfig.CreateMapper());

//var config = new ConfigurationBuilder()
//    .SetBasePath(AppContext.BaseDirectory)
//    .AddJsonFile("appsettings.json")
//    .Build();

//var connectionString = GetConnectionSetup(config);

var app = builder.Build();

//var scope = app.Services.CreateScope();
//await ServiceExtension.ManageDataAsync(scope.ServiceProvider);

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{

//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
