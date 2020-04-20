using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TCSDemoProjectAlcoaWebApi.Model {
	public class CityDetails {

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long cityid { get; set; }
		public string cityname { get; set; }
		public bool active_status { get; set; }
		public DateTime createddate { get; set; }
		
		public long stateidfk { get; set; }
		//[ForeignKey("cityidfk")]
		//public ICollection<UsersDetailInfo> UsersDetailInfo { get; set; }
	}
}