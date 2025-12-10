using mongo_api.Entities;
using mongo_api.Repositories;
using mongo_api.Utils;

var builder = WebApplication.CreateBuilder(args);

// OpenAPI / Swagger 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.EnableAnnotations();
});

builder.Services.Configure<MongoEntity>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<MongoRepository>();
builder.Services.AddSingleton<Redis>();

// JWT

/*
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.Authority = builder.Configuration["JWT:Authority"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"]!,
    };
});*/

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();