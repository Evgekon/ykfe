using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using kyfelib;

namespace kyfewww.Areas.Admin.Controllers
{
    public class EditContentController : Controller
    {
        private IDataHelper _dataHelper;

		public EditContentController()
		{
			var storageAccount = CloudStorageAccount.Parse(
				CloudConfigurationManager.GetSetting("StorageConnectionString"));
			_dataHelper = new AzureDataHelper(storageAccount);
		}

		public EditContentController(IDataHelper dataHelper)
		{
			_dataHelper = dataHelper;
		}

		public ActionResult Index(string id)
		{
			if (!string.IsNullOrEmpty(id))
				return View(_dataHelper.ContentGet(id));
			return View();
		}
    }
}
