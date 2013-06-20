using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kyfelib
{
    public interface IDataHelper
	{
		#region Tag
		Tag TagGet(int id);
	    List<Tag> TagGetList();
		Tag TagCreate(Tag tag);
	    bool TagUpdate(Tag tag);
		bool TagDelete(int id);
	    #endregion

		#region Image

		string ImageCreate(string name);
	    bool ImageDelete(string name);

	    #endregion

		#region Article
		Article ArticleGet(string id);
		List<Article> ArticleGetList();
	    Article ArticleCreate(Article newArticle);
	    bool ArticleUpdate(Article updateArticle);
		bool ArticleDelete(Article deleteArticle);
		#endregion
	}
}
