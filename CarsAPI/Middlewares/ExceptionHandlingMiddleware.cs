namespace CarsAPI.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "CSV file not found");

                context.Response.StatusCode = 404;
                context.Response.ContentType = "application/json";

                var result = new
                {
                    message = "The CSV data file was not found.",
                    details = GetErrorDetails(ex)
                };
                await context.Response.WriteAsJsonAsync(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Access denied to the CSV file");

                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";

                var result = new
                {
                    message = "Access to the CSV file is denied.",
                    details = GetErrorDetails(ex)
                };
                await context.Response.WriteAsJsonAsync(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled error occurred");

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var result = new
                {
                    message = "An unexpected error occurred. Please try again later.",
                    details = GetErrorDetails(ex)
                };
                await context.Response.WriteAsJsonAsync(result);
            }
        }

        private string GetErrorDetails(Exception ex)
        {
            return _env.IsDevelopment() ? ex.ToString() : "Internal Server Error";
        }
    }

}
