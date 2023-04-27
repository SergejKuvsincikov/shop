using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;

namespace API.MiddleWare
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> logger;
        public IHostEnvironment Environment { get; }
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
        {
            this.Environment = environment;
            this.logger = logger;
            this.next = next;
            
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try 
            {
                await next(context);
            }
            catch(Exception ex)
            {
                logger.LogError(ex,ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode .InternalServerError;

                var response = Environment.IsDevelopment() ?
                    new ApiException((int)HttpStatusCode .InternalServerError, ex.Message,ex.StackTrace.ToString()):
                    new ApiException((int)HttpStatusCode .InternalServerError, ex.Message);

                    var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

                    var json = JsonSerializer.Serialize(response,options);

                    await context.Response.WriteAsync(json);
            }
        }
    }
}