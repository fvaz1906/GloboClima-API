using GloboClima.Application;

var builder = WebApplication.CreateBuilder(args);

Startup.Configure(builder.Services, builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("PolicyDefault");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
