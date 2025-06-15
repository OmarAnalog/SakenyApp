using SakennyProject.Erorrs;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace SakennyProject.Middlewares
{
    public class ExceptionMiddlleWare
    {
        public RequestDelegate Next { get; }
        public ILogger<ExceptionMiddlleWare> Logger { get; }
        public IHostEnvironment Env { get; }
        public ExceptionMiddlleWare(RequestDelegate next, ILogger<ExceptionMiddlleWare> logger, IHostEnvironment env)// لازم يكون موجو ctor  بياخد   ,RequestDelegate , ILogger
        {
            Next = next;
            Logger = logger;
            Env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                await Next.Invoke(context);
                stopWatch.Stop();
                Logger.LogInformation($"Request {context.Request.Path} took {stopWatch.ElapsedMilliseconds} ms");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = Env.IsDevelopment() ? new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString()) : new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);
                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var JsonResponse = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(JsonResponse);
            }

        }
    }
}
