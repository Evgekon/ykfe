using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using kyfelib;

namespace kyfewww.Controllers
{
	public class ArticleController : Controller
	{
		private IDataHelper _dataHelper;

		public ArticleController()
		{
			var storageAccount = CloudStorageAccount.Parse(
				CloudConfigurationManager.GetSetting("StorageConnectionString"));
			_dataHelper = new AzureDataHelper(storageAccount);
		}

		public ArticleController(IDataHelper dataHelper)
		{
			_dataHelper = dataHelper;
		}

		public ActionResult Index(string id, string l)
		{
			if (string.IsNullOrEmpty(id))
				return View(_dataHelper.ArticleGetList());

			var article = _dataHelper.ArticleGet(id);
			return article != null 
				? View("Article", _dataHelper.ArticleGet(id)) 
				: View(_dataHelper.ArticleGetList());
		}

	}
}
