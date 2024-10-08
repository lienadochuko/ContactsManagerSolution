using Contact_Manager.Filters.ActionFilters;
using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.Core.Services;
using ContactsManager.Infastructure;
using ContactsManager.Infastructure.Repositories;
using ContactsManager.Infastructure.Repositories.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Contact_Manager
{
	public static class ConfigureServicesExtention
	{
		public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
		{
			//services.AddMvc();

			services.AddControllersWithViews(options => {
				var logger = services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();

				//options.Filters.Add(new ResponseHeaderActionFilter("My-Key-From-Global", "My-Value-From-Global", 3));
				options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
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
			services.AddScoped<IDataRepository, DataRepository>();
			services.AddScoped<IMoviesRepository, MoviesRepository>();
			services.AddScoped<IMoviesGetterServices, MoviesGetterService>();
			services.AddScoped<IMoviesUpdaterServices, MoviesUpdaterService>();


			services.AddDbContext<ApplicationDbContext>
				(
				options =>
				{
					options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
				}
			);

			//Enable Identity in this project
			services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
			{
				//options.SignIn.RequireConfirmedEmail = true;
				options.Password.RequiredLength = 8;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireDigit = true;
				options.Password.RequiredUniqueChars = 3; //Eg: AB12AB (unique characters are A,B,1,2)
				options.Lockout.AllowedForNewUsers = true;  // Enable lockout for new users
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);  // Set lockout duration to 10 minutes
				options.Lockout.MaxFailedAccessAttempts = 10;  // Allow 3 failed attempts before lockout
			}).AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders()
			.AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
			.AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

			services.AddAuthorization(options =>
			{
				options.FallbackPolicy = new AuthorizationPolicyBuilder()
				.RequireAuthenticatedUser().Build();
				//enforces authorization policy (user must be authenticated) for all action methods

				//Custom Policy
				options.AddPolicy("NotAuthorized", policy =>
				{
					policy.RequireAssertion(context =>
					{
						return !context.User.Identity.IsAuthenticated;
					});
				}); 
			});

			services.ConfigureApplicationCookie(option =>
			{
				option.LoginPath = "/Auth/Login";
			});

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
