using Microsoft.AspNetCore.Mvc.Filters;

namespace Contact_Manager.Filters.ResultFilters
{
	public class TokenResultFilter : IAsyncResultFilter
	{

		public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
		{
			context.HttpContext.Response.Cookies.Append("Account-Key", "A100");

			await next();

			
		}
	}
}
