using Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

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

            if (exception is RegisterFailedException)
            {
                _logger.LogError(exception.Message);
                problemDetails = CreateProblemDetails(400, "Register Failed", exception.Message);
            }
            else if (exception is UnauthorizedException)
            {
                _logger.LogError(exception.Message);
                problemDetails = CreateProblemDetails(401, "Invalid information", exception.Message);
            }
            else if (exception is ForbiddenException)
            {
                _logger.LogError(exception.Message);
                problemDetails = CreateProblemDetails(403, "Forbidden request", exception.Message);
            }
            else if (exception is DbSavingFailedException)
            {
                _logger.LogError(exception.Message);
                problemDetails = CreateProblemDetails(500, "Database saving failed", exception.Message);
            }
            else if (exception is NotFoundException)
            {
                _logger.LogError(exception.Message);
                problemDetails = CreateProblemDetails(404, "Not found", exception.Message);
            }
            else if (exception is RoleAssignFailedException)
            {
                _logger.LogError(exception.Message);
                problemDetails = CreateProblemDetails(500, "Role Assign failed", exception.Message);
            }
            else if (exception is AlreadyExistsException)
            {
                _logger.LogError(exception.Message);
                problemDetails = CreateProblemDetails(400, "Already exists", exception.Message);
            }
            else if (exception is UnsupportedFileTypeException)
            {
                _logger.LogError(exception.Message);
                problemDetails = CreateProblemDetails(400, "Unsupported file type", exception.Message);
            }
            else
            {
                return;
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
