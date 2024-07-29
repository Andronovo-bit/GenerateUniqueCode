using FluentValidation;
using FluentValidation.AspNetCore;
using GenerateCampaignCode.Application.Interfaces;
using GenerateCampaignCode.Application.Requests;
using GenerateCampaignCode.Application.Services;
using GenerateCampaignCode.Application.Validators;
using GenerateCampaignCode.Domain.Entities;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
        .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<GenerateCodeRequestValidator>())
        .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ValidateCodeRequestValidator>());
builder.Services.Configure<CampaignCodeSettings>(builder.Configuration.GetSection("CampaignCodeSettings"));
builder.Services.AddSingleton<ICodeGenerator, CodeGenerator>();


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

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
