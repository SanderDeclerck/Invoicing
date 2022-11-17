var builder = WebApplication.CreateBuilder(args);

builder.Environment.ApplicationName = "Invoicing Service";

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddTelemetry();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseReDoc(config =>
    {
        config.DocumentTitle = "Invoicing api";
    });
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();
