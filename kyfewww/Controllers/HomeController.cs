using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using kyfelib;


namespace kyfewww.Controllers
{
	public class HomeController : Controller
	{
		private IDataHelper _dataHelper;

		public HomeController()
		{
			var storageAccount = CloudStorageAccount.Parse(
				CloudConfigurationManager.GetSetting("StorageConnectionString"));
			_dataHelper = new AzureDataHelper(storageAccount);
		}

		public HomeController(IDataHelper dataHelper)
		{
			_dataHelper = dataHelper;
		}


		public ActionResult Index()
		{
			return View();
		}

		public ActionResult ArticleEdit(string id)
		{
			if (!string.IsNullOrEmpty(id))
				return View(_dataHelper.ArticleGet(id));
			return View();


		}
	}
}
