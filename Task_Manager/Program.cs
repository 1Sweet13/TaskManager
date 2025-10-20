using Microsoft.EntityFrameworkCore;
using Task_Manager;
using Task_Manager.Data;
using Task_Manager.Repositories;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<EmployeesRepository>();
builder.Services.AddScoped<AssignmentRepository>();
builder.Services.AddScoped<ProjectsRepository>();
builder.Services.AddScoped<GroupRepository>();

builder.Services.AddDbContext<AssignmentManagerDBContext>(option =>
{
    option.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddProblemDetails();

var app = builder.Build();


// SeedData.Initialize(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
