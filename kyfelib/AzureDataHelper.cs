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

		#region Content

		public ContentModel ContentGet(string id)
		{
			var table = _tableClient.GetTableReference("Content");
			var query = new TableQuery<Content>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id));
			var result = table.ExecuteQuery(query);

			var content = FillContentModel(result).First();

			return content;
		}

		public List<ContentModel> ContentGetList()
		{
			var table = _tableClient.GetTableReference("Content");
			var query = new TableQuery<Content>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.NotEqual, string.Empty));
			var result = table.ExecuteQuery(query);

			var contList = FillContentModel(result);
			return contList;
		}

		public List<ContentModel> ContentGetList(ContentType type)
		{
			var table = _tableClient.GetTableReference("Content");
			var query =
				new TableQuery<Content>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal,
				                                                                   Enum.GetName(typeof (ContentType), type)));
			var result = table.ExecuteQuery(query);
			var contList = FillContentModel(result);
			return contList;
		}

		public ContentModel ContentCreate(ContentModel newArticle)
		{
			var table = _tableClient.GetTableReference("Content");
			table.CreateIfNotExists();

			var con = new Content(newArticle.Type);
			var tableOperation = TableOperation.Insert(con);
			table.Execute(tableOperation);

			foreach (var locale in newArticle.Locales)
			{
				var loc = new ContentLocale(locale.Locale)
					          {
						          ContentId = con.RowKey,
						          Name = locale.Name,
						          Author = locale.Author,
						          IsDraft = locale.IsDraft,
						          Text = locale.Text
					          };
				if (locale.Tags.Any())
					loc.Tags = locale.Tags.Aggregate((workingSentence, next) => workingSentence + ";" + next);
				ContentLocaleCreate(loc);
			}
			newArticle.Id = con.RowKey;
			newArticle.Date = DateTime.ParseExact(con.RowKey, "yyyyMMddHHmmssffff",
															  System.Globalization.CultureInfo.InvariantCulture);
			return newArticle;
		}

		public void ContentLocaleCreate(ContentLocale locale)
		{
			var table = _tableClient.GetTableReference("ContentLocale");
			table.CreateIfNotExists();
			var tableOperation = TableOperation.Insert(locale);
			table.Execute(tableOperation);
		}

		public bool ContentUpdate(ContentModel updateArticle)
		{
			var table = _tableClient.GetTableReference("Content");
			var query = new TableQuery<Content>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, updateArticle.Id));
			var res = table.ExecuteQuery(query).First();
			var locTable = _tableClient.GetTableReference("ContentLocale");
			var locQuery =
					new TableQuery<ContentLocale>().Where(TableQuery.GenerateFilterCondition("ContentId", QueryComparisons.Equal,
																							 res.RowKey));
			var locRes = locTable.ExecuteQuery(locQuery).ToList();
			foreach (var loc in locRes.Where(l=>!updateArticle.Locales.Exists(ul=>Enum.GetName(typeof(Locale), ul.Locale).Equals(l.PartitionKey))))
			{
				var tableOperation = TableOperation.Delete(loc);
				table.Execute(tableOperation);
			}

			foreach (var locale in updateArticle.Locales)
			{
				var locStr = Enum.GetName(typeof (Locale), locale.Locale);
				var con = new ContentLocale(locale.Locale)
					          {
								  PartitionKey = locStr,
								  ContentId = res.RowKey,
						          Name = locale.Name,
						          Author = locale.Author,
						          IsDraft = locale.IsDraft,
						          Text = locale.Text,
						          Tags = locale.Tags.Any()
							                 ? locale.Tags.Aggregate((workingSentence, next) => workingSentence + ";" + next)
							                 : null
					          };
				if (locRes.Any(l => l.PartitionKey.Equals(locStr)))
					con.RowKey = locRes.Single(l => l.PartitionKey.Equals(locStr)).RowKey;

				var tableOperation = TableOperation.InsertOrReplace(con);
				locTable.Execute(tableOperation);
			}

			return true;
		}

		public bool ContentDelete(ContentModel deleteArticle)
		{
			var table = _tableClient.GetTableReference("Content");
			var query = new TableQuery<Content>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, deleteArticle.Id));
			foreach (var element in table.ExecuteQuery(query))
			{
				var locTable = _tableClient.GetTableReference("ContentLocale");
				var locQuery =
					new TableQuery<ContentLocale>().Where(TableQuery.GenerateFilterCondition("ContentId", QueryComparisons.Equal,
																							 element.RowKey));

				foreach (var loc in locTable.ExecuteQuery(locQuery))
				{
					var locOp = TableOperation.Delete(loc);
					locTable.Execute(locOp);
				}

				var tableOperation = TableOperation.Delete(element);
				table.Execute(tableOperation);
			}

			return true;
		}

		private List<ContentModel> FillContentModel(IEnumerable<Content> contents)
		{
			var contList = new List<ContentModel>();

			foreach (var c in contents)
			{
				var content = new ContentModel
					              {
						              Id = c.RowKey,
									  Date =
							              DateTime.ParseExact(c.RowKey, "yyyyMMddHHmmssffff",
							                                  System.Globalization.CultureInfo.InvariantCulture),
						              Type = (ContentType) Enum.Parse(typeof (ContentType), c.PartitionKey)
					              };
				content.Locales.AddRange(ContentLocaleGet(c.RowKey));
				contList.Add(content);
			}

			return contList;
		}

		public List<ContentModelLocale> ContentLocaleGet(string contentId)
		{
			var table = _tableClient.GetTableReference("ContentLocale");
			var query =
				new TableQuery<ContentLocale>().Where(TableQuery.GenerateFilterCondition("ContentId", QueryComparisons.Equal,
																						 contentId));
			var result = table.ExecuteQuery(query);
			var contLocList = new List<ContentModelLocale>();

			contLocList.AddRange(result.Select(loc => new ContentModelLocale
				                                          {
					                                          Id = loc.RowKey,
					                                          Locale = (Locale) Enum.Parse(typeof (Locale), loc.PartitionKey),
					                                          Name = loc.Name,
					                                          Text = loc.Text,
					                                          Author = loc.Author,
					                                          IsDraft = loc.IsDraft,
					                                          Tags =
						                                          (!string.IsNullOrEmpty(loc.Tags))
							                                          ? loc.Tags.Split(';')
							                                               .Where(t => !string.IsNullOrEmpty(t))
							                                               .ToList()
							                                          : new List<string>()
				                                          }));

			return contLocList;
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

		public bool UserValidate(string email, string password)
		{
			var user = UserGet(email);
			return GetPasswordHash(password).Equals(user.Password);
		}

		public string GetPasswordHash(string password)
		{
			var x = new System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] bs = Encoding.UTF8.GetBytes(password);
			bs = x.ComputeHash(bs);
			var s = new StringBuilder();
			foreach (byte b in bs)
			{
				s.Append(b.ToString("x2").ToLower());
			}
			string res = s.ToString();
			return res;
		}
		#endregion
	}
}
