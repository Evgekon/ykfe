using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using kyfelib;
using kyfelib.Users;

namespace kyfewww.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private IDataHelper _dataHelper;

		public LoginController()
		{
			var storageAccount = CloudStorageAccount.Parse(
				CloudConfigurationManager.GetSetting("StorageConnectionString"));
			_dataHelper = new AzureDataHelper(storageAccount);
		}

		public LoginController(IDataHelper dataHelper)
		{
			_dataHelper = dataHelper;
		}

        public ActionResult Index()
        {
            return View();
        }

		[HttpPost]
		public ActionResult Index(string email, string password)
		{
			var user = _dataHelper.UserGet(email);

			if (user!=null && user.RowKey.Equals(_dataHelper.GetPasswordHash(password)))
				SignIn(email, true);
			return View();
		}

		public void SignIn(string email, bool createPersistentCookie)
		{
			FormsAuthentication.SetAuthCookie(email, createPersistentCookie);
			FormsAuthentication.RedirectFromLoginPage(User.Identity.Name, createPersistentCookie);
		}
		public void SignOut()
		{
			FormsAuthentication.SignOut();
		}
    }
}
