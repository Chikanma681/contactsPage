using Microsoft.EntityFrameworkCore;
using ContactsPage.Data;
using FluentValidation;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Add ProblemDetailsFactory (required by our filter)
// builder.Services.AddProblemDetails();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddDbContext<ContactsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider;
    SeedData.Seed(dbContext);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

public partial class Program 
{ 

}