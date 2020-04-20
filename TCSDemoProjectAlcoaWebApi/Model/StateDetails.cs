using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TCSDemoProjectAlcoaWebApi.Model {
	public class StateDetails {

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long stateid { get; set; }

		[Required]
		public string statename { get; set; }
		public bool active_status { get; set; }
		public DateTime createddate { get; set; }

		public long countryidfk { get; set; }

		[ForeignKey("stateidfk")]
		public ICollection<CityDetails> CityDetails { get; set; }
		//[ForeignKey("stateidfk")]
		//public ICollection<UsersDetailInfo> UsersDetailInfo { get; set; }
	}
}