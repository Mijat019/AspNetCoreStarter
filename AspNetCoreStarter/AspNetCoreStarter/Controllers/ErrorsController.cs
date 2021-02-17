using AspNetCoreStarter.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace AspNetCoreStarter.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        private struct ErrorResponse
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }

        }

        [Route("error")]
        public void Error()
        {
            IExceptionHandlerFeature context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            Exception exception = context?.Error;

            HttpContext.Response.ContentType = "application/json";

            int status = 500;
            object errors = new { error = "Error occurred while processing your request." };

            if (exception is BusinessException businessException)
            {
                status = businessException.StatusCode;
                errors = new { error = businessException.Message };
            }
            else if (exception != null)
            {
                status = Response.StatusCode;
                errors = new { error = exception.Message };
            }

            HttpContext.Response.StatusCode = status;
            HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(new { errors, status }));
        }

        [Route("error-development")]
        public void ErrorDevelopment()
        {
            IExceptionHandlerPathFeature context = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            Exception ex = context?.Error;

            ErrorResponse errorResponse = this.GetErrorFromException(ex, Response.StatusCode);

            HttpContext.Response.ContentType = "application/json";
            HttpContext.Response.StatusCode = errorResponse.StatusCode;

            if (ex != null)
            {
                HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    context.Path,
                    errors = new { error = ex.Message },
                    status = errorResponse.StatusCode,
                    ex.StackTrace,
                    ex.InnerException
                }));
            }
            else
            {
                HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            }
        }

        private ErrorResponse GetErrorFromException(Exception exception, int statusCode)
        {
            if (exception is BusinessException businessException) return this.HandleBussinesException(businessException);

            if (exception != null) return this.HandleOtherException(exception);

            // Default error
            return new ErrorResponse
            {
                StatusCode = statusCode,
                Message = "Error occurred while processing your request."
            };
        }

        private ErrorResponse HandleBussinesException(BusinessException businessException)
        {
            return new ErrorResponse
            {
                Message = businessException.Message,
                StatusCode = businessException.StatusCode
            };
        }

        private ErrorResponse HandleOtherException(Exception exception)
        {
            return new ErrorResponse
            {
                StatusCode = 500,
                Message = exception.Message
            };
        }
    }
}
