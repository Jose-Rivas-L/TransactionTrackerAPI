using Microsoft.AspNetCore.HttpLogging;
using TransactionTrackerAPI.Resources;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add HTTP logging
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders
    | HttpLoggingFields.Response;
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

var app = builder.Build();

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
            string response = responseBody.Contains("errors") ? responseBody : "";
            string responseInfo = $" System log\nMethod: {context.Request.Method}\n" +
                $"Path: {context.Request.Path}\n" +
                $"StatusCode: {context.Response.StatusCode}\n" +
                $"Content-Type: {context.Response.ContentType}\n" +
                $"Response Body: {response}";

            LogToFile logging = new LogToFile();
            logging.Log(responseInfo);
        }
    }
    finally
    {
        context.Response.Body = originalBody;
    }
});

app.UseRouting();

app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseEndpoints(endpoints =>
    {
        // Redirect to Swagger when accessing the root path
        endpoints.MapGet("/", context =>
        {
            context.Response.Redirect("/swagger/index.html");
            return Task.CompletedTask;
        });

        endpoints.MapControllers();
    });
}
else
{
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}

app.Run();
