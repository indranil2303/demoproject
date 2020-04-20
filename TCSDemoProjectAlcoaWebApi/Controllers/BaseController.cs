using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TCSDemoProjectAlcoaWebApi.Model;

namespace TCSDemoProjectAlcoaWebApi.Controllers
{
	[Produces("application/json")]
    
	[Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
		private Resultmodel re;
		private readonly ModelContext __context;
		
		public BaseController(ModelContext context){
			this.__context = context;
		}

		partial class userdetail {
			
			public long userid { get; set; }
			public string name { get; set; }
			public string userole { get; set; }
			public string contact { get; set; }

			public string cityname { get; set; }
			public string statename { get; set; }
			public string countryname { get; set; }
			public bool active_status { get; set; }
		}
		public partial class login {

			[Required]
			[EmailAddress]
			public string email { get; set; }

			[Required]
			[DataType(DataType.Password)]
			public string password { get; set; }

			public bool isremember { get; set; }
		}
		
		private bool CheckForMailExitance(string email){
			try {

				return __context.UsersDetailInfo
					.Where(xx => xx.email.Equals(email)).FirstOrDefault() != null ? true : false;
			}
			catch(Exception) {

				throw;
			}
		}

		/* Login Section */

		[HttpPost]
		[Route("~/api/login")]
		public IActionResult Login([FromBody] login info){

			try {

				if(!ModelState.IsValid){

					re = new Resultmodel()
					{
						statuscode = (long)HttpStatusCode.Forbidden,
						error_message = "Email & password parameters are mandatory."
					};
					
					return new ObjectResult(re);
				}

				if(!CheckForMailExitance(info.email)){

					re = new Resultmodel()
					{
						statuscode = (long)HttpStatusCode.Forbidden,
						error_message = "Email does not exist."
					};
					
					return new ObjectResult(re);
				}

				var obj = __context.UsersDetailInfo.Where(xx => xx.email.Equals(info.email) 
					&& xx.active_status == false).FirstOrDefault();
				if(obj != null) {

					re = new Resultmodel()
					{
						statuscode = (long)HttpStatusCode.Forbidden,
						error_message = "Account is not activated yet."
					};
					
					return new ObjectResult(re);
				}

				obj = __context.UsersDetailInfo.Where(xx => xx.email.Equals(info.email)
					&& xx.password.Equals(info.password)).FirstOrDefault();
				if(obj == null){

					re = new Resultmodel()
					{
						statuscode = (long)HttpStatusCode.Forbidden,
						error_message = "Password does not match."
					};
					
					return new ObjectResult(re);
				}
				
				userdetail usrdata = (from ss in __context.UsersDetailInfo
						  where ss.email.Equals(info.email) &&
								ss.password.Equals(info.password)
						  select new userdetail(){
							userid = ss.userid,
							  name = ss.firstname + '^' + ss.lastname,

							  contact = ss.mobileno,
							  cityname = (from xx in __context.CityDetails where xx.cityid == ss.cityidfk select xx.cityname).Single<string>(),
							  statename = (from xx in __context.StateDetails where xx.stateid == ss.stateidfk select xx.statename).Single<string>(),
							  countryname = (from xx in __context.CountryDetails where xx.countryid == ss.countryidfk select xx.countryname).Single<string>(),

							  userole = (from xx in __context.UserRoles where xx.roleid == ss.roleidfk select xx.rolename).Single<string>()
						  }).First<userdetail>();

				
				re = new Resultmodel()
				{
					statuscode = (long)HttpStatusCode.OK,
					info = usrdata
				};
				
				return new ObjectResult(re);
			}
			catch(Exception e) {

				re = new Resultmodel()
				{
					statuscode = (long)HttpStatusCode.InternalServerError,
					error_message = "something went wrong."
				};

				return new ObjectResult(re);
			}
			finally {
				__context.Dispose();
			}
		}

		/* Registration Section */
		
		[HttpGet]
		[Route("~/api/getcountry")]
		public IActionResult GetAllCountry() {
			try{

				var result = __context.CountryDetails.ToArray<CountryDetails>();
				re = new Resultmodel()
				{
					statuscode = (long)HttpStatusCode.OK,
					info = result
				};

				return new ObjectResult(re);
			}
			catch(Exception){

				re = new Resultmodel()
				{
					statuscode = (long)HttpStatusCode.InternalServerError,
					error_message = "something went wrong."
				};
					
				return new ObjectResult(re);
			}
		}

		[HttpGet]
		[Route("~/api/getstate/{id}")]
		public IActionResult GetState(long id) {
			try{

				var result = __context.StateDetails.Where(xx=>xx.countryidfk == id)
					.ToArray<StateDetails>();

				re = new Resultmodel()
				{
					statuscode = (long)HttpStatusCode.OK,
					info = result
				};

				return new ObjectResult(re);
			}
			catch(Exception){

				re = new Resultmodel()
				{
					statuscode = (long)HttpStatusCode.InternalServerError,
					error_message = "something went wrong."
				};
					
				return new ObjectResult(re);
			}
		}

		[HttpGet]
		[Route("~/api/getcity/{id}")]
		public IActionResult GetCity(long id){
			try{

				var result = __context.CityDetails.Where(xx=>xx.stateidfk == id)
					.ToArray<CityDetails>();

				re = new Resultmodel()
				{
					statuscode = (long)HttpStatusCode.OK,
					info = result
				};

				return new ObjectResult(re);
			}
			catch(Exception){

				re = new Resultmodel()
				{
					statuscode = (long)HttpStatusCode.InternalServerError,
					error_message = "something went wrong."
				};
					
				return new ObjectResult(re);
			}
		}

		[HttpPost]
		[Route("~/api/register")]
		public IActionResult Register([FromBody] UsersDetailInfo info) {
			try {

				if(!ModelState.IsValid){

					re = new Resultmodel()
					{
						statuscode = (long)HttpStatusCode.Forbidden,
						error_message = "Failed^All parameters are mandatory."
					};
					
					return new ObjectResult(re);
				}

			if(CheckForMailExitance(info.email)){

				re = new Resultmodel()
				{
					statuscode = (long)HttpStatusCode.Forbidden,
					error_message = "Failed^Email already exists in the database."
				};
					
				return new ObjectResult(re);
			}

			__context.UsersDetailInfo.Add(info);
			__context.SaveChanges();

			re = new Resultmodel()
			{
				statuscode = (long)HttpStatusCode.OK,
				info = "Success^Successfully registered."
			};
					
			return new ObjectResult(re);
			}
			catch(Exception){

				re = new Resultmodel()
				{
					statuscode = (long)HttpStatusCode.InternalServerError,
					error_message = "Failed^Something went wrong."
				};
					
				return new ObjectResult(re);
			}
		}

		/* Admin Section */

		[HttpGet]
		[Route("~/api/getalluser/{roleid}")]
		public IActionResult GetAllUser(long roleid){
			try {

				userdetail[] userinfo = (from ss in __context.UsersDetailInfo
						  where ss.roleidfk != roleid
						  select new userdetail(){
							userid = ss.userid,
							  name = ss.firstname + '^' + ss.lastname,

							  contact = ss.mobileno,
							  cityname = (from xx in __context.CityDetails where xx.cityid == ss.cityidfk select xx.cityname).Single<string>(),
							  statename = (from xx in __context.StateDetails where xx.stateid == ss.stateidfk select xx.statename).Single<string>(),
							  countryname = (from xx in __context.CountryDetails where xx.countryid == ss.countryidfk select xx.countryname).Single<string>(),

							  userole = (from xx in __context.UserRoles where xx.roleid == ss.roleidfk select xx.rolename).Single<string>(),

							  active_status = ss.active_status
						  }).ToArray<userdetail>();

				re = new Resultmodel()
				{
					statuscode = (long)HttpStatusCode.OK,
					info = userinfo
				};
					
				return new ObjectResult(re);

			}
			catch(Exception) {

				re = new Resultmodel()
				{
					statuscode = (long)HttpStatusCode.InternalServerError,
					error_message = "something went wrong."
				};
					
				return new ObjectResult(re);
			}
		}

		[HttpGet]
		[Route("~/api/allowuser/{id}")]
		public IActionResult AllowUser(long id){

			try {

				if(__context.UsersDetailInfo.Where(xx => xx.userid == id)
						.FirstOrDefault().active_status){

					__context.UsersDetailInfo.Where(xx => xx.userid == id)
						.FirstOrDefault().active_status = false;
				}
				else
				{
					__context.UsersDetailInfo.Where(xx => xx.userid == id)
						.FirstOrDefault().active_status = true;
					
				}

				__context.SaveChanges();

				re = new Resultmodel()
				{
					statuscode = (long)HttpStatusCode.OK,
					info = "success"
				};
					
				return new ObjectResult(re);
			}
			catch(Exception) {

				re = new Resultmodel()
				{
					statuscode = (long)HttpStatusCode.InternalServerError,
					error_message = "something went wrong."
				};
					
				return new ObjectResult(re);
			}
		}

		/* Test Section */

		[AcceptVerbs("GET", "POST")]
		[Route("~/api/getvalue")]

		public IActionResult GetTestValue([FromBody] GetValuesQueryParameters param) => Ok(new {
			name = "indranil",
			age = "24",
			bloodgroup = "A+"
		});

		public partial class GetValuesQueryParameters
		{
			[Required] public DateTime From { get; set; }
			[Required] public DateTime To { get; set; }
		}
	}
}