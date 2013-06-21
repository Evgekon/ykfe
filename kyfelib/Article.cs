using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;

namespace kyfelib
{
	public class Article : TableEntity
	{
		public Article()
		{
			Locales = new List<ArticleContent>();
		}

		public List<ArticleContent> Locales { get; set; }
	}
}
