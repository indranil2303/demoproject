using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TCSDemoProjectAlcoa.Models {
	public class AppModel {

	}

	public partial class Login 
	{

		[Required(ErrorMessage = "Please enter email")]
		[EmailAddress(ErrorMessage = "Please enter a valid enail address")]
		public string email { get; set; }

		[Required(ErrorMessage = "Please enter password")]
		[DataType(DataType.Password)]
		public string password { get; set; }

		public bool isremember { get; set; }
	}

	public partial class Registration 
	{
		
		[Required(ErrorMessage = "Please enter email")]
		[EmailAddress(ErrorMessage = "Please enter a valid enail address")]
		public string email { get; set; }

		[Required(ErrorMessage = "Please enter password")]
		[MinLength(4,ErrorMessage = "Password should contain atleast 4 characters")]
		[DataType(DataType.Password)]
		public string password { get; set; }

		[Required(ErrorMessage = "Please enter first name")]
		public string firstname { get; set; }

		[Required(ErrorMessage = "Please enter lastname")]
		public string lastname { get; set; }

		[Required(ErrorMessage = "Please enter contact number")]
		[MinLength(10,ErrorMessage = "Contact number should contain atleast 10 digits")]
		public string mobileno { get; set; }

		public long roleidfk { get; set; }

		[Required(ErrorMessage = "Please select city")]
		public long cityidfk { get; set; }

		[Required(ErrorMessage = "Please select state")]
		public long stateidfk { get; set; }

		[Required(ErrorMessage = "Please select country")]
		public long countryidfk { get; set; }

		public DateTime createdtime { get; set; }
		//public List<CountryDetail> countrydetails { get; set; }
	}

	public partial class CountryDetail {

		public long countryid { get; set; }
		public string countryname { get; set; }
		
		public bool active_status { get; set; }
		public DateTime createddate { get; set; }
	}
	public partial class StateDetail {
		public long stateid { get; set; }
		public string statename { get; set; }
		public bool active_status { get; set; }
		public DateTime createddate { get; set; }
		public long countryidfk { get; set; }
	}
	public partial class CityDetail {

		public long cityid { get; set; }
		public string cityname { get; set; }
		public bool active_status { get; set; }
		public DateTime createddate { get; set; }
		
		public long stateidfk { get; set; }
	}
	public partial class ReturnModel {
		
		public long statuscode { get; set; }
		public Object info { get; set; }
		public string error_message { get; set; }
	}
	
	public partial class UserDetail {

		public long userid { get; set; }
		public string name { get; set; }
		public string userole { get; set; }
		public string contact { get; set; }

		public string cityname { get; set; }
		public string statename { get; set; }
		public string countryname { get; set; }

		public bool active_status { get; set; }
	}
}
