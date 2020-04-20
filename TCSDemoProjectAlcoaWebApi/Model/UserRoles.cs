using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TCSDemoProjectAlcoaWebApi.Model {
	public class UserRoles {

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long roleid { get; set; }
		public string rolename { get; set; }
		public bool active_status { get; set; }
		public DateTime createddate { get; set; }
		
		[ForeignKey("roleidfk")]
		public ICollection<UsersDetailInfo> UsersDetailInfo { get; set; }
	}
}
