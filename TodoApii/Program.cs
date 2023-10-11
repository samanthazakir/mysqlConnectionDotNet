using Microsoft.EntityFrameworkCore;
using System.Configuration;
using TodoApi.Models;
using TodoApii.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));
//builder.Services.AddDbContext<TimetableDbContext>(options =>
//    options.UseMySQL("Server=127.0.0.1;Database=timetable;User=root;Password=1234;"));
builder.Services.AddDbContext<TimetableDbContext>(service =>
    service.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));
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
