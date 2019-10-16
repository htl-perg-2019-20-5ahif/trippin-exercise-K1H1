using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TrippinUser
{

	public class FileUser
	{
	
		public string UserName { get; set; }

		
		public string FirstName { get; set; }

		public string LastName { get; set; }

		
		public string Emails { get; set; }

		
		public string Address { get; set; }

		
		public string CityName { get; set; }

		
		public string Country { get; set; }

	
	}

	public class FileUsers
	{
		
		public List<FileUser> fileUsers { get; set; }
	}

}
