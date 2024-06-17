using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml.Style;
using static OfficeOpenXml.ExcelErrorValue;

namespace Contact_Manager.Filters.ActionFilters
{

	public class ResponseHeaderFilterFactoryAttribute : Attribute, IFilterFactory
	{
		public bool IsReusable => false;
		private string _key { get; set; }
		private string _value { get; set; }

		private int Order { get; set; }


		public ResponseHeaderFilterFactoryAttribute( string key, string value, int order)
		{
			_key = key;
			_value = value;
			Order = order;
		}

		//Controller -> FilterFactory -> Filter
		public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
		{
			return new ResponseHeaderActionFilter(_key, _value, Order);
		}
	}
	public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
	{
		private readonly string _key;
		private readonly string _value;

		public int Order { get; set; }

		public ResponseHeaderActionFilter(string key, string value, int order)
		{
			_key = key;
			_value = value;
			Order = order;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			//before

			await next();//calls the subsequent filter or action method

			//after
			context.HttpContext.Response.Headers[_key] = _value;
		}
	}
}
