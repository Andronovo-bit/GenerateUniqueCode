using GenerateCampaignCode.Application.Interfaces;
using GenerateCampaignCode.Application.Services;
using GenerateCampaignCode.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
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
