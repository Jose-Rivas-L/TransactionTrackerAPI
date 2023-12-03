using Microsoft.AspNetCore.HttpLogging;
using TransactionTrackerAPI.Resources;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders
    | HttpLoggingFields.Response;
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHttpLogging();

app.Use(async (context, next) =>
{
    var originalBody = context.Response.Body;
    try
    {
        using (var memoryStream = new MemoryStream())
        {
            context.Response.Body = memoryStream;

            await next();

            memoryStream.Seek(0, SeekOrigin.Begin);
            await memoryStream.CopyToAsync(originalBody);

            string responseBody = Encoding.UTF8.GetString(memoryStream.ToArray());

            string responseInfo = $" System log\nMethod: {context.Request.Method}\n" +
                $"Path: {context.Request.Path}\n" +
                $"StatusCode: {context.Response.StatusCode}\n" +
                $"Content-Type: {context.Response.ContentType}\n" +
                $"Response Body: {responseBody}";

            LogToFile logging = new LogToFile();
            logging.Log(responseInfo);
        }
    }
    finally
    {
        context.Response.Body = originalBody;
    }
});

app.UseAuthorization();

app.MapControllers();

app.Run();
