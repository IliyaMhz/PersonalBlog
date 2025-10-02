using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersonalBlog.Api.Models.DataBaseContext;
using PersonalBlog.Api.Profile;
using PersonalBlog.Api.Service.Interfaces;
using PersonalBlog.Api.Service.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173")  
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddDbContext<AppDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("AppConnection")));
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<BlogProfile>();
});
builder.Services.AddScoped<IBlogService, BlogService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
#region pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors("AllowReactApp");
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
#endregion
app.Run();
