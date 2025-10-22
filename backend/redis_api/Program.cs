using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// OpenAPI / Swagger 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.EnableAnnotations();
});

// Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(
    new ConfigurationOptions {
        EndPoints = {builder.Configuration["Redis:EndPoint"]!},
        User = builder.Configuration["Redis:User"]!,
        Password = builder.Configuration["Redis:Password"]!,
    }
));

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();