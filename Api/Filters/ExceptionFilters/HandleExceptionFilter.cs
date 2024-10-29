using Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Api.Filters.ExceptionFilters
{
    public class HandleExceptionFilter : IExceptionFilter
    {
        private readonly ProblemDetailsFactory _problemDetailsFactory;
        private readonly ILogger<HandleExceptionFilter> _logger;
        private readonly IHttpContextAccessor _contextAccessor;

        public HandleExceptionFilter(ProblemDetailsFactory problemDetailsFactory, ILogger<HandleExceptionFilter> logger, IHttpContextAccessor contextAccessor)
        {
            _problemDetailsFactory = problemDetailsFactory;
            _logger = logger;
            _contextAccessor = contextAccessor;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var problemDetails = new ProblemDetails();

            if (context.Exception is RegisterFailedException)
            {
                _logger.LogError(context.Exception.Message);
                problemDetails = CreateProblemDetails(500, "Register Failed", exception.Message);
            }
            else
            {
                throw context.Exception;
            }

            context.Result = new ObjectResult(problemDetails);
            context.ExceptionHandled = true;
        }

        private ProblemDetails CreateProblemDetails(int statusCode, string title, string detail)
        {
            HttpContext context = _contextAccessor.HttpContext!;
            return _problemDetailsFactory.CreateProblemDetails(context, statusCode, title, detail: detail);
        }
    }
}
