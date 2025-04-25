using Microsoft.EntityFrameworkCore;
using TaskHabitApi.Models;

var filePath = Path.GetFullPath("../.env");

if (File.Exists(filePath))
{
    foreach (var line in File.ReadAllLines(filePath))
    {
        var parts = line.Split(
            '=',
            StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 2)
            continue;

        Environment.SetEnvironmentVariable(parts[0], parts[1]);
    }
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>  
{  
    options.AddPolicy(name: "_myAllowSpecificOrigins",  
        policy  =>  
        {  
            policy.WithOrigins("http://localhost:80",  
                "http://justnotes.xyz")
                .AllowAnyHeader()
                .AllowAnyMethod(); // add the allowed origins  
        });  
}); 

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySQL(connectionString);
});

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();
app.UseCors("_myAllowSpecificOrigins");  
app.Run();
