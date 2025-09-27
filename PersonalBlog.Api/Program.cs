using Microsoft.Extensions.DependencyInjection;
using PersonalBlog.Api.Profile;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<BlogProfile>();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
#region pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
#endregion
app.Run();
