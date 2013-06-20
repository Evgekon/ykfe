using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using kyfelib;

namespace kyfewww.Controllers
{
    public class EditController : ApiController
    {
        private IDataHelper _dataHelper;

		public EditController()
		{
			var storageAccount = CloudStorageAccount.Parse(
				CloudConfigurationManager.GetSetting("StorageConnectionString"));
			_dataHelper = new AzureDataHelper(storageAccount);
		}

		public EditController(IDataHelper dataHelper)
		{
			_dataHelper = dataHelper;
		}


		// GET api/article
		public IEnumerable<Article> Get()
		{
			return _dataHelper.ArticleGetList();
		}

		// GET api/article/5
		public Article Get(string id)
		{
			return _dataHelper.ArticleGet(id);
		}

		// Create POST api/article
		public Article Post(Article article)
		{
			var newArticle = new Article(article.PartitionKey, article.Name, article.Locale, article.Text, article.Author);

			return _dataHelper.ArticleCreate(newArticle);
		}

		// Update PUT api/article/5
		public bool Put(Article article)
		{
			var art = _dataHelper.ArticleGet(article.RowKey);

			return art != null && _dataHelper.ArticleUpdate(art);
		}

		// DELETE api/article/5
		public bool Delete(string id)
		{
			var art = _dataHelper.ArticleGet(id);
			return art != null && _dataHelper.ArticleDelete(art);
		}
    }
}
