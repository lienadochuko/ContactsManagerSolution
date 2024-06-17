using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Contact_Manager.Filters.ResourceFilters
{
	public class FeatureDisableldResourceFilter : IAsyncResourceFilter
	{
		private readonly ILogger<FeatureDisableldResourceFilter> _logger;

		private readonly bool _isDisabled;

		public FeatureDisableldResourceFilter(ILogger<FeatureDisableldResourceFilter> logger, bool isDisabled)
		{
			_logger = logger;
			_isDisabled = isDisabled;
		}

		public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
		{
			//TO DO: before logic
			_logger.LogInformation("{FilterName}.{MethodName} - before", nameof(FeatureDisableldResourceFilter), nameof(OnResourceExecutionAsync));
			
			if(_isDisabled)
			{
				//context.Result = new NotFoundResult();

				context.Result = new StatusCodeResult(501); //Not Implemented

			}
			else
			{
			await next();
			}

			//TO DO: after logic
			_logger.LogInformation("{FilterName}.{MethodName} - before", nameof(FeatureDisableldResourceFilter), nameof(OnResourceExecutionAsync));
		}
	}
}
