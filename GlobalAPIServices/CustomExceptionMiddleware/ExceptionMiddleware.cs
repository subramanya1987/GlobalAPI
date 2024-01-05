using GlobalAPIServices.LoggerService;
using GlobalAPIServices.Model;
using System.Net;

namespace GlobalAPIServices.CustomExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;

        public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
        {
           _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong :{ex}");
                await HandleExceptionHandler(httpContext,ex);
            }
        }

        private Task HandleExceptionHandler(HttpContext httpContext,Exception ex)
        {
            httpContext.Response.ContentType = "application/json";
            //httpContext.Response.StatusCode = httpContext.Response.StatusCode;
            //httpContext.Response.StatusCode= httpContext.Response.StatusCode;
            if (httpContext.Response.StatusCode == 200)
            {
                httpContext.Response.StatusCode = 500;
            }
            return httpContext.Response.WriteAsync(new ErrorDetails
            {
               
                StatusCode = httpContext.Response.StatusCode,
                Message = ex.Message
            }.ToString());
        }
    }
}
