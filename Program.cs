using bookpj.Repository;
using bookpj.Service;
using bookpj.Services;
using bookpj.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<IBookRepository, BookRepository>();

// Đăng ký tầng Service liên kết với Repository
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddValidatorsFromAssemblyContaining<BookValidator>();
builder.Services.AddControllers();
// 2. Đăng ký DataContext vào hệ thống Dependency Injection (DI)
builder.Services.AddDbContext<bookpj.Extension.DataContext>(options =>
    options.UseSqlServer(connectionString));
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
