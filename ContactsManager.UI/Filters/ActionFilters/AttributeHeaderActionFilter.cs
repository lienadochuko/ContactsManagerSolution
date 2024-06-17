using Microsoft.AspNetCore.Mvc.Filters;

namespace Contact_Manager.Filters.ActionFilters
{
    public class AttributeHeaderActionFilter : ActionFilterAttribute
    {
        private readonly string _key;
        private readonly string _value;


        public AttributeHeaderActionFilter(string key, string value, int order)
        {
            _key = key;
            _value = value;
            Order = order;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //before

            await next();//calls the subsequent filter or action method

            //after
            context.HttpContext.Response.Headers[_key] = _value;
        }
    }
}
