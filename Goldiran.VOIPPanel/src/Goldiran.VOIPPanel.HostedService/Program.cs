using Goldiran.VOIPPanel.HostedService;
using Goldiran.VOIPPanel.HostedService.BackGroundService.IServices;
using Goldiran.VOIPPanel.HostedService.BackGroundService.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddSingleton<IFlatDataJobService, FlatDataJobService>();
builder.Services.AddSingleton<IExecuteFlatDataService, ExecuteFlatDataService>();
builder.Services.AddSingleton<IContactDetailsService, ContactDetailsService>();

builder.Services.AddHostedService<FlatBackgroundService>();
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
