using Acerola.Domain;
using Acerola.Infrastructure;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Newtonsoft.Json;

using System.Net;

namespace Acerola.WebApi.Filters
{
    public sealed class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            string json;

            if (context.Exception is DomainException domainException)
            {
                json = JsonConvert.SerializeObject(domainException.Message);
                context.Result = new BadRequestObjectResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (context.Exception is ApplicationException applicationException)
            {
                json = JsonConvert.SerializeObject(applicationException.Message);
                context.Result = new BadRequestObjectResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (context.Exception is InfrastructureException infrastructureException)
            {
                json = JsonConvert.SerializeObject(infrastructureException.Message);
                context.Result = new BadRequestObjectResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                json = JsonConvert.SerializeObject(context.Exception.Message);
                context.Result = new ObjectResult(json) { StatusCode = (int)HttpStatusCode.InternalServerError };
            }
        }
    }
}