using CashBox.Service.Services.ConractorAccountServices;
using CashBox.Service.Services.ContractorService;
using CashBox.Service.Services.CorrencyRateServices;
using CashBox.Service.Services.CorrencyServices;
using CashBox.Service.Services.DistrictServices;
using CashBox.Service.Services.OrganizationServices;
using CashBox.Service.Services.RegionServices;
using CashBox.Service.Services.UserServices;
using Microsoft.EntityFrameworkCore;
using Repository.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IDistrictService, DistrictService>();
builder.Services.AddScoped<ICurrencyRateService, CurrencyRateService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<IContractorService, ContractorService>();
builder.Services.AddScoped<IContratorAccountService, ContratorAccountService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
