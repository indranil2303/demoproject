using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TCSDemoProjectAlcoa.Models;

namespace TCSDemoProjectAlcoa.Controllers {
	public class HomeController : Controller {
		
		private WebApi __api;
		private string __apiurl;
		private IConfiguration Iconfig;

		public HomeController(IConfiguration config){
			this.Iconfig = config;
			
			__apiurl = config.GetValue<string>("ApiURL");
			__api = new WebApi();
		}

		[Authorize(Policy = "Policy.User")]
		[Route("~/Home/Dashboard")]

		public IActionResult Dashboard() {

			return View();
		}


		[Authorize]
		public IActionResult Logout(){

			HttpContext.SignOutAsync();
			return RedirectPermanent("~/Home/Login");
		}

		/* Login */

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Login(){

			if(User.Identity.IsAuthenticated){

				if(User.IsInRole("admin")) {

					return RedirectPermanent("~/Home/AdminDashboard");
				}
				else if(User.IsInRole("user")){

					return RedirectPermanent("~/Home/Dashboard");
				}
			}

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Login([Bind] Login info){
			try{

				if(!ModelState.IsValid){

					return Redirect("~/Home/Login");	
				}
				else
				{
					var response = Task.Run<System.Net.Http.HttpResponseMessage>(()=>
					__api.PostReq(this.__apiurl+"api/login", info)).Result;

					if(response.IsSuccessStatusCode){

						var res = JsonConvert.DeserializeObject<ReturnModel>(
						response.Content.ReadAsStringAsync().Result);
						var resp = new UserDetail();
						
						if(!string.IsNullOrEmpty(res.error_message)) {

							//ModelState.AddModelError("error",res.error_message);

							ViewBag.Message = res.error_message;
							return View();
						}
						else
						{
							resp = JsonConvert.DeserializeObject<UserDetail>(
								JsonConvert.SerializeObject(res.info));

							var claims = new List<Claim>
							{
								new Claim("userid",Convert.ToString(resp.userid)),
								new Claim(ClaimTypes.Name,resp.name),
								new Claim(ClaimTypes.Role,resp.userole)
							};

							var identity = new ClaimsIdentity(claims,
								CookieAuthenticationDefaults.AuthenticationScheme);
							
							var principal = new ClaimsPrincipal(identity);

							HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
								principal);
							
						}

						/* Redirecting dashboard based on assigned roles */
						if(resp.userole.Equals("admin")) {

							return RedirectPermanent("~/Home/AdminDashboard");
						}
						else if(resp.userole.Equals("user")) {

							return RedirectPermanent("~/Home/Dashboard");
						}

						//ModelState.AddModelError("error","You don't not permission yet.");

						ViewBag.Message = "You don't not permission yet";
						return View();
					}
					else 
					{
						//ModelState.AddModelError("error","Something went wrong");
						
						ViewBag.Message = "Something went wrong";
						return View();
					}
				}
			}
			catch(Exception) {

				ViewBag.Message = "Something went wrong";
				return View();
			}
		}


		/* Registration */
		public IActionResult Registration(){
			try 
			{

				if(User.Identity.IsAuthenticated){
					if(User.IsInRole("admin")) {

						return RedirectPermanent("~/Home/AdminDashboard");
					}
					else if(User.IsInRole("user")){

						return RedirectPermanent("~/Home/Dashboard");
					}
				}
			}
			catch(Exception) {

			}

			return View();
		}

		[Route("~/Home/GetCountries")]
		public IActionResult GetCountries() {

			List<CountryDetail> countrydetails = new List<CountryDetail>();
			try {

				var res = Task.Run<System.Net.Http.HttpResponseMessage>(()=>
				__api.GetReq(this.__apiurl+"api/getcountry")).Result;

				var resp = JsonConvert.DeserializeObject<ReturnModel>(
					res.Content.ReadAsStringAsync().Result);

				countrydetails = JsonConvert.DeserializeObject<List<CountryDetail>>(
					JsonConvert.SerializeObject(resp.info));
			}
			catch(Exception) {
				return null;
			}

			return Json(countrydetails);
		}

		[Route("~/Home/GetStates/{id}")]
		public IActionResult GetStates(long id) {

			List<StateDetail> statedetail = new List<StateDetail>();
			try {
				var res = Task.Run<System.Net.Http.HttpResponseMessage>(() => 
				__api.GetReq(this.__apiurl+"api/getstate/" + Convert.ToString(id))).Result;

				var resp = JsonConvert.DeserializeObject<ReturnModel>(res.Content.ReadAsStringAsync().Result);
				statedetail = JsonConvert.DeserializeObject<List<StateDetail>>(JsonConvert.SerializeObject(resp.info));
			}
			catch(Exception) 
			{

			}

			return Json(statedetail);
		}

		[Route("~/Home/GetCities/{id}")]
		public IActionResult GetCities(long id) {

			List<CityDetail> citydetail = new List<CityDetail>();
			try {
				var res = Task.Run<System.Net.Http.HttpResponseMessage>(() => 
				__api.GetReq(this.__apiurl+"api/getcity/" + Convert.ToString(id))).Result;

				var resp = JsonConvert.DeserializeObject<ReturnModel>(res.Content.ReadAsStringAsync().Result);
				citydetail = JsonConvert.DeserializeObject<List<CityDetail>>(JsonConvert.SerializeObject(resp.info));
			}
			catch(Exception) 
			{

			}

			return Json(citydetail);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Registration([Bind] Registration reg) {
			if(!ModelState.IsValid) {

				//ModelState.AddModelError("","Please check all the mandatory fields");
				return Redirect("~/Home/Registration");
			}
			else 
			{
				reg.roleidfk = 2;
				reg.createdtime = DateTime.Now;

				var res = Task.Run<System.Net.Http.HttpResponseMessage>(() => __api.PostReq(this.__apiurl+"api/register", reg)).Result;
				var resp = JsonConvert.DeserializeObject<ReturnModel>(res.Content.ReadAsStringAsync().Result);
				if(!string.IsNullOrEmpty(resp.error_message)) {

					//ModelState.AddModelError("error", resp.error_message);

					ViewBag.Message = resp.error_message;
					return View();
				}
				else
				{
					ViewBag.Message = (string)resp.info;
					return View();
				}
			}
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() {
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		/* Admin */
		
		[Route("~/Home/ToggleStatus/{id}")]
		[Authorize(Policy = "Policy.Admin")]
		public IActionResult ToggleStatus(long id){

			try{
				var res = Task.Run<HttpResponseMessage>(() =>
				__api.GetReq(this.__apiurl+"api/allowuser/" + Convert.ToString(id))).Result;

				if(!res.IsSuccessStatusCode){

					return Ok("{ status : failed }");
				}
			}
			catch(Exception) {

				return Ok("{ status : failed }");
			}

			return Ok("{ status : success }");
		}

		[Authorize(Policy = "Policy.Admin")]
		[Route("~/Home/AdminDashboard")]

		public IActionResult AdminDashboard(){

			List<UserDetail> userdetail = new List<UserDetail>();
			try {

				var res = Task.Run<HttpResponseMessage>(() => 
				__api.GetReq(this.__apiurl+"api/getalluser/"+Convert.ToString(1))).Result;
				var resp = JsonConvert.DeserializeObject<ReturnModel>(res.Content.ReadAsStringAsync().Result);
				userdetail = JsonConvert.DeserializeObject<List<UserDetail>>(
					JsonConvert.SerializeObject(resp.info));  
				
				//var re = HttpContext.User.Claims.FirstOrDefault(xx => xx.Type == "userid").Value;
			}
			catch(Exception) {

				throw;
			}

			return View(userdetail);
		}
	}
}
