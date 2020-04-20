using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TCSDemoProjectAlcoaWebApi.Model {
	public class Resultmodel {

		public long statuscode { get; set; }
		public Object info { get; set; }
		public string error_message { get; set; }
	}
}
