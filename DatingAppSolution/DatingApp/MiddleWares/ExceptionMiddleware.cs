using DatingApp.Errors;
using System.Net;
using System.Text.Json;

namespace DatingApp.MiddleWares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware( RequestDelegate next, IHostEnvironment env)
        {
            _next = next;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext request)
        {
            try
            {
               await _next.Invoke(request);
            }
            catch (Exception e)
            {
                request.Response.StatusCode= (int)HttpStatusCode.InternalServerError;
                request.Response.ContentType= "application/json";
                var option = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var error = _env.IsDevelopment() ? new ServerError() { Detail = e.StackTrace??"Some Server Errors!", Message = e.Message }:
                                                   new ServerError();
               await request.Response.WriteAsync(JsonSerializer.Serialize(error,option));
            }
        }
        
    }
}
