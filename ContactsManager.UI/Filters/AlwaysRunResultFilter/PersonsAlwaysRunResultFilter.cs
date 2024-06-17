using Microsoft.AspNetCore.Mvc.Filters;

namespace Contact_Manager.Filters.AlwaysRunResultFilter 
{
	public class PersonsAlwaysRunResultFilter : IAlwaysRunResultFilter

	{
		public void OnResultExecuted(ResultExecutedContext context)
		{
		}

		public void OnResultExecuting(ResultExecutingContext context)
		{
			//TO DO: before logic
			//if(context.HttpContext.Request.Path.Value.Contains("Persons"))
			if(context.Filters.OfType<SkipFilter>().Any())
			{
				return;
			}
		}
	}
}
