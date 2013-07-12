using System.Collections.Generic;
using System.Web.Http;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using kyfelib;

namespace kyfewww.Areas.Admin.Controllers
{
	//[Authorize(Roles = "Manager")]
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


		// GET api/edit
		public IEnumerable<ContentModel> Get()
		{
			return _dataHelper.ContentGetList();
		}

		// GET api/edit/5
		public ContentModel Get(string id)
		{
			return _dataHelper.ContentGet(id);
		}

		// Create POST api/edit
		public ContentModel Post(ContentModel content)
		{
			return _dataHelper.ContentCreate(content);
		}

		// Update PUT api/edit/
		public ContentModel Put(ContentModel content)
		{
			var c = _dataHelper.ContentGet(content.Id);

			return (c != null && _dataHelper.ContentUpdate(content)) ? content : null;
		}

		// DELETE api/edit/5
		public bool Delete(string id)
		{
			var c = _dataHelper.ContentGet(id);
			return c != null && _dataHelper.ContentDelete(c);
		}
    }
}
