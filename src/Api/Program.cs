using FluentValidation.AspNetCore;
using GenerateCampaignCode.Application.Interfaces;
using GenerateCampaignCode.Application.Services;
using GenerateCampaignCode.Application.Validators;
using GenerateCampaignCode.Domain.Entities;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
//Add support to logging with SERILOG
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddControllers()
        .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<GenerateCodeRequestValidator>())
        .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ValidateCodeRequestValidator>());

builder.Services.Configure<CampaignCodeSettings>(builder.Configuration.GetSection("CampaignCodeSettings"));
builder.Services.AddScoped<ICodeGenerator, CodeGenerator>();

// built in way to validate scopes on Build()
builder.Host.UseDefaultServiceProvider((_, serviceProviderOptions) =>
{
    serviceProviderOptions.ValidateScopes = true;
    serviceProviderOptions.ValidateOnBuild = true;
});

builder.Services.AddMemoryCache();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();

Log.CloseAndFlush();
