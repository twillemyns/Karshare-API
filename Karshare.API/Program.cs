using Karshare.API.Data;
using Microsoft.EntityFrameworkCore;
using Karshare.API.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.InjectDepencies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();