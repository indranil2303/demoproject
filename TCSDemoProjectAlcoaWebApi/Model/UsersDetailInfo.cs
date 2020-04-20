using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TCSDemoProjectAlcoaWebApi.Model {
	public class UsersDetailInfo {

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long userid { get; set; }
		
		[Required]
		[EmailAddress]
		public string email { get; set; }

		[Required]
		public string password { get; set; }

		public string firstname { get; set; }
		public string lastname { get; set; }
		public string mobileno { get; set; }

		public bool active_status { get; set; }
		public DateTime createdtime { get; set; }

		public long roleidfk { get; set; }
		public long cityidfk { get; set; }
		public long stateidfk { get; set; }

		public long countryidfk { get; set; }
	}
}
