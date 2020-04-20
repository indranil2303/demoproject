using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TCSDemoProjectAlcoa {
	public class Startup {
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{

					options.Cookie.Name = "__AuthCookie";
					options.LoginPath = "/Home/Login";

					options.ExpireTimeSpan = TimeSpan.FromDays(1);
					options.SlidingExpiration = true;
				});

			services.AddAuthorization(options =>
			{
				options.AddPolicy("Policy.Admin",
					policy => policy.RequireAuthenticatedUser()
						.RequireRole("admin")
						.Build());
			});

			services.AddAuthorization(options =>
			{
				options.AddPolicy("Policy.User",
					policy => policy
						.RequireAuthenticatedUser()
						.RequireRole("user")
						.Build());
			});

			services.AddRouting(options => {
				options.LowercaseUrls = true;
			});

			services.AddControllersWithViews();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if(env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}
			else {
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseCookiePolicy();

			app.UseEndpoints(endpoints =>
			{
				//endpoints.MapDefaultControllerRoute();

				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Login}");
			});
		}
	}
}
