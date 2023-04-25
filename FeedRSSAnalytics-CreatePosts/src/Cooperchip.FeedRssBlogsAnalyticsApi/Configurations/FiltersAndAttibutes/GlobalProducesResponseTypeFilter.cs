using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.Configurations.FiltersAndAttibutes
{
    public class GlobalProducesResponseTypeFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if(context.Result is ObjectResult objectResult) 
            {
                objectResult.StatusCode = (int)HttpStatusCode.OK; // 200
                objectResult.ContentTypes.Add("application/json");
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if(!context.ModelState.IsValid) 
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
            if(context.Result is NotFoundResult) 
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }

            if (context.Result is ObjectResult objectResult)
            {
                objectResult.StatusCode = (int)HttpStatusCode.OK; // 200
                objectResult.ContentTypes.Add("application/json");
            }

        }
    }
}
