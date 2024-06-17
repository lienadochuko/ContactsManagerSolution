using Contact_Manager.Filters.ActionFilters;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.Core.Services;
using ContactsManager.Infastructure;
using ContactsManager.Infastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Contact_Manager
{
	public static class ConfigureServicesExtention
	{
		public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration )
		{
			//services.AddMvc();

			services.AddControllersWithViews(options => {
				var logger = services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();

				//options.Filters.Add(new ResponseHeaderActionFilter("My-Key-From-Global", "My-Value-From-Global", 3));
			});

			//add services into IoC container
			services.AddScoped<ICountriesService, CountriesService>();
			services.AddScoped<IPersonGetterServices, PersonsGetterServiceWithFewExcelFields>();
			services.AddScoped<PersonGetterService, PersonGetterService>();
			services.AddScoped<IPersonAdderServices, PersonAdderService>();
			services.AddScoped<IPersonDeleterServices, PersonDeleterService>();
			services.AddScoped<IPersonUpdaterServices, PersonUpdaterService>();
			services.AddScoped<IPersonSorterServices, PersonSorterService>();
			services.AddScoped<ICountriesRepository, CountriesRepository>();
			services.AddScoped<IPersonRepository, PersonsRepository>();


			services.AddDbContext<ApplicationDbContext>
				(
				options =>
				{
					options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
				}
			);

			services.AddHttpLogging(options =>
			{
				options.LoggingFields =
				Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties |
				Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestPropertiesAndHeaders;
			});

			return services;
		}
	}
}
