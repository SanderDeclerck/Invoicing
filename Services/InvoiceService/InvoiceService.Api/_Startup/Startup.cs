using Invoicing.Services.InvoiceService.Api.Invoices;

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
}

app.UseHttpsRedirection();

app.MapGroup("/invoices")
   .MapInvoiceApi()
   .WithTags("Invoices");

app.Run();
