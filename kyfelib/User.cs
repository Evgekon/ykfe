using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace kyfelib
{
	public class User : TableEntity
	{
		public User() { }

		public User(string name, string email, string groupname, string password)
		{
			PartitionKey = groupname;
			RowKey = email;
			Name = name;
			Password = password;
		}

		public string Email { get; set; }
		public string Name { get; set; }
		public string Password { get; set; }
	}
}
