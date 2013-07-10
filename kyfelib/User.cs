using Microsoft.WindowsAzure.Storage.Table;

namespace kyfelib
{
	public class User : TableEntity
	{
		public User() { }

		public User(string name, string email, string group, string password)
		{
			PartitionKey = group;
			RowKey = email;
			Name = name;
			Password = password;
		}

		public string Name { get; set; }
		public string Password { get; set; }
		public string Provider { get; set; } //Local/External provider
		public string ProviderUid { get; set; } //External providers only
	}
}
