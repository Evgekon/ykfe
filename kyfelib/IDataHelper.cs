using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kyfelib
{
    public interface IDataHelper
	{
		#region Image
		string ImageCreate(string name);
	    bool ImageDelete(string name);
	    #endregion

		#region Content
	    ContentModel ContentGet(string id);
		List<ContentModel> ContentGetList();
	    List<ContentModel> ContentGetList(ContentType type);
		ContentModel ContentCreate(ContentModel newArticle);
		bool ContentUpdate(ContentModel updateArticle);
		bool ContentDelete(ContentModel deleteArticle);
		#endregion

		#region User
	    User UserGet(string id);
	    List<User> UserGetList();
	    User UserCreate(User newUser);
	    bool UserUpdate(User updateUser);
	    bool UserDelete(User deleteUser);
	    string GetPasswordHash(string password);
	    #endregion
	}
}
