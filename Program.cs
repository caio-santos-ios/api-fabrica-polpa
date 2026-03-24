using PulpaAPI.src.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddBuilderConfiguration();
builder.AddBuilderAuthentication();
builder.AddContext();
builder.AddBuilderServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:3000",
                Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "http://localhost:3000"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
