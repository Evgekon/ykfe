using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using CloudStorageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount;

namespace kyfelib
{
	public class AzureDataHelper : IDataHelper
	{
		private CloudTableClient _tableClient;
		private CloudBlobClient _blobClient;

		public AzureDataHelper(CloudStorageAccount storageAccount)
		{
			_tableClient = storageAccount.CreateCloudTableClient();
			_blobClient = storageAccount.CreateCloudBlobClient();
		}

		#region Tag
		public Tag TagGet(int id)
		{
			throw new NotImplementedException();
		}

		public List<Tag> TagGetList()
		{
			throw new NotImplementedException();
		}

		public Tag TagCreate(Tag tag)
		{
			throw new NotImplementedException();
		}

		public bool TagUpdate(Tag tag)
		{
			throw new NotImplementedException();
		}

		public bool TagDelete(int id)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Image
		public string ImageCreate(string name)
		{
			throw new NotImplementedException();
		}

		public bool ImageDelete(string name)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Article
		public Article ArticleGet(string id)
		{
			var table = _tableClient.GetTableReference("article");
			var tableOperation = TableOperation.Retrieve<Article>("Article", id);
			var result = table.Execute(tableOperation);

			return result.Result == null ? null : result.Result as Article;
		}

		public List<Article> ArticleGetList()
		{
			var table = _tableClient.GetTableReference("article");
			var query =
				new TableQuery<Article>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Article"));

			return table.ExecuteQuery(query).ToList();
		}

		public Article ArticleCreate(Article newArticle)
		{
			var table = _tableClient.GetTableReference("article");
			table.CreateIfNotExists();
			var tableOperation = TableOperation.Insert(newArticle);
			table.Execute(tableOperation);

			return newArticle;
		}

		public bool ArticleUpdate(Article updateArticle)
		{
			var table = _tableClient.GetTableReference("article");
			var tableOperation = TableOperation.Replace(updateArticle);
			table.Execute(tableOperation);

			return true;
		}

		public bool ArticleDelete(Article deleteArticle)
		{
			var table = _tableClient.GetTableReference("article");
			var tableOperation = TableOperation.Delete(deleteArticle);

			table.Execute(tableOperation);

			return true;
		}
		#endregion

		#region User
		public User UserGet(string id)
		{
			var table = _tableClient.GetTableReference("user");
			var tableOperation = TableOperation.Retrieve("User", id);
			var result = table.Execute(tableOperation);

			return result.Result == null ? null : result.Result as User;
		}

		public List<User> UserGetList()
		{
			var table = _tableClient.GetTableReference("user");
			var query =
				new TableQuery<User>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "User"));

			return table.ExecuteQuery(query).ToList();
		}

		public User UserCreate(User newUser)
		{
			var table = _tableClient.GetTableReference("user");
			table.CreateIfNotExists();
			var tableOperation = TableOperation.Insert(newUser);
			table.Execute(tableOperation);

			return newUser;
		}

		public bool UserUpdate(User updateUser)
		{
			var table = _tableClient.GetTableReference("user");
			var tableOperation = TableOperation.Replace(updateUser);
			table.Execute(tableOperation);

			return true;
		}

		public bool UserDelete(User deleteUser)
		{
			var table = _tableClient.GetTableReference("user");
			var tableOperation = TableOperation.Delete(deleteUser);

			table.Execute(tableOperation);

			return true;
		}
		#endregion
	}
}
