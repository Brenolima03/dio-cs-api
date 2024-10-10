using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<OrganizadorContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

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
try
{
    // Resolving the OrganizadorContext (DI-based context) to test the connection
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<OrganizadorContext>();

        // Simple query to test the connection and see if the Users table can be queried
        var users = context.Tarefas.ToList(); // This will check if the connection works
        Console.WriteLine("Connection successful. Users found: " + users.Count);
    }
}
catch (Exception ex)
{
    Console.WriteLine("Connection failed: " + ex.Message);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
