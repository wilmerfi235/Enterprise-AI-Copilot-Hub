using EnterpriseAI.API.Hubs;
using EnterpriseAI.Application;
using EnterpriseAI.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// --- Añadir los servicios de las capas mediante las extensiones ---
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddSignalR(); // Habilitar SignalR para tiempo real
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

app.MapHub<AgentChatHub>("/hubs/agentChat");

app.Run();

public partial class Program { }
   
