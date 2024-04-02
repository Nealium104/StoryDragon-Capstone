using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using StoryDragon.Data;
using StoryDragon.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173")
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StoryDragonContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("StoryDragonConnection")));

builder.Services.AddAuthorization();
builder.Services.AddAuthorization();

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<StoryDragonContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();