using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieBookingAuthApi.Interfaces;
using MovieBookingAuthApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ITokenService, TokenService>();


//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(builder =>
//    {
//        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
//    });
//});

builder.Services.AddCors(o=>o.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    .WithExposedHeaders("content-type");
}));

builder.Services.AddControllers();

//builder.Services.Configure<MongoDbSettings>(
//    builder.Configuration.GetSection("MongoDb"));

builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection("MongoDbConfig"));

builder.Services.AddSingleton<IMongoDbConfig>(sp => sp.GetRequiredService<IOptions<MongoDbConfig>>().Value);

builder.Services.AddSingleton<IMongoClient>(mongoConnection => new MongoClient(builder.Configuration.GetValue<string>("MongoDbConfig:ConnectionString")));

builder.Services.AddSingleton<MongoDbContext>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("CorsPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}


//app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));


//app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
