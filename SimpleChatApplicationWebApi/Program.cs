using Microsoft.EntityFrameworkCore;
using SimpleChatApplication.BLL.Services;
using SimpleChatApplication.BLL.Services.Interfaces;
using SimpleChatApplication.BLL.SignalR;
using SimpleChatApplication.DAL;
using SimpleChatApplication.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSignalR();

builder.Services.AddDbContext<SimpleChatAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"), opt =>
    {
        opt.MigrationsAssembly(typeof(SimpleChatAppDbContext).Assembly.GetName().Name);
    }));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

await app.SeedDataAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chatHub");

app.Run();
