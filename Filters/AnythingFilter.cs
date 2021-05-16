using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AuthGold.Contracts;
using AuthGold.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthGold.Filters
{
    public class AnythingFilter : IAsyncActionFilter
    {

        private readonly IElapsedTime _elapsedtime;
        private readonly IJsonManipulate _jsonManipulate;
        private readonly IRequestTrace _requestTrace;
        private Stopwatch Stopwatch { get; set; }
        public AnythingFilter(
            IElapsedTime elapsedTime,
            IJsonManipulate jsonManipulate,
            IRequestTrace requestTrace
        )
        {
            _elapsedtime = elapsedTime;
            _jsonManipulate = jsonManipulate;
            _requestTrace = requestTrace;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Stopwatch = _elapsedtime.Open();
            Stopwatch.Start();

            var ContextController = await next();

            var elapsedtime = _elapsedtime.Close(Stopwatch);
            
            if(ContextController.Controller is ControllerBase controller)
            {
                var reqTrace = new RequestTrace
                {
                    id = Guid.NewGuid().ToString(),
                    address = UriHelper.GetDisplayUrl(controller.Request),
                    clientCode = Guid.NewGuid().ToString(),
                    elapsedTime = elapsedtime,
                    httpMethod = controller.Request.Method,
                    httpStatusCode = controller.Response.StatusCode,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                await _requestTrace.Create(reqTrace);

                _jsonManipulate.WriteDecryptedJson("C:\\Users\\Patricia\\Documents\\RequestTrace.yur");
            }
        }
    }
}
