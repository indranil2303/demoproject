using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TCSDemoProjectAlcoaWebApi.Model {
	public class CountryDetails {

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long countryid { get; set; }

		public string countryname { get; set; }
		
		public bool active_status { get; set; }
		public DateTime createddate { get; set; }

		[ForeignKey("countryidfk")]
		public ICollection<StateDetails> StateDetails { get; set; }
		//[ForeignKey("countryidfk")]
		//public ICollection<UsersDetailInfo> UsersDetailInfo { get; set; }
	}
}
