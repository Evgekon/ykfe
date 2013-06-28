using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace kyfelib
{
	public class ArticleContent : TableEntity
	{
		public string Name { get; set; }
		public Locale Locale { get; set; }
		public string Text { get; set; }
		public string Author { get; set; }
		public bool IsDraft { get; set; }

		public ArticleContent()
		{
		}

		public ArticleContent(string articleId)
		{
			PartitionKey = articleId;
			RowKey = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
		}
	}
}
