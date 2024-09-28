using CrudWithMongoDB.Model;
using CrudWithMongoDB.Service;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// Register the MongoDB database as a singleton
builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

// Register the MongoDB collection for Stocks
builder.Services.AddSingleton<IMongoCollection<Stocks>>(sp =>
{
    var database = sp.GetRequiredService<IMongoDatabase>();
    return database.GetCollection<Stocks>(nameof(Stocks)); // or the collection name you want
});

builder.Services.AddSingleton<IMongoCollection<Orders>>(sp =>
{
    var database = sp.GetRequiredService<IMongoDatabase>();
    return database.GetCollection<Orders>(nameof(Orders)); // Collection name for Orders
});

builder.Services.AddSingleton<BaseService<Stocks>>();
builder.Services.AddSingleton<StocksService>();
builder.Services.AddSingleton<OrderService>();
builder.Services.AddSingleton<MongoRepository>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(x => x.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
}));

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

app.UseCors("CorsPolicy");

app.Run();
