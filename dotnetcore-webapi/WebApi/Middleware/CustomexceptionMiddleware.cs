using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebApi
{
    public class CustomexceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomexceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var watch = Stopwatch.StartNew();
            try
            {
                
                string message = "[Request]  HTTP " + context.Request.Method + " - " + context.Request.Path ;
                System.Console.WriteLine(message);
                await _next(context); 
                watch.Stop();
                message = "[Response] HTTP " +  context.Request.Method + " - " + context.Request.Path + " responded " + context.Response.StatusCode 
                        + " in " + watch.ElapsedMilliseconds + " ms " ;
                System.Console.WriteLine(message);
            }
            catch (Exception ex)
            {
               watch.Stop();
               await HandleException(context,ex,watch);
            }
         }

        private Task HandleException(HttpContext context, Exception ex,Stopwatch watch)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            string message = "[Error] HTTP " +  context.Request.Method + " - " +  context.Response.StatusCode + " Error Message "
                + ex.Message + " in " + watch.ElapsedMilliseconds + " ms " ;
            System.Console.WriteLine(message);

            var result = JsonConvert.SerializeObject(new {error = ex.Message} , Formatting.None);
            return context.Response.WriteAsync(result);
        }
    }
    public static class CustomexceptionMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomExceptionMiddle(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomexceptionMiddleware>();
        }
    }

}