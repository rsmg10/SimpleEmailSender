using EmailSender;
using EmailSender.Library.Installers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureEmailSender();
builder.Services.AddControllers(
    options=>
    {
        options.InputFormatters.Add(new BypassFormDataInputFormatter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
