using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace kyfelib
{
	public class Article : TableEntity
	{
		public Article() { }

		public Article(string articleType, string name, int locale, string text, string author)
		{
			PartitionKey = articleType;
			RowKey = DateTime.Now.ToString("yyyyMMddHHmmssffff");
			Name = name;
			Locale = locale;
			Text = text;
			Author = author;
		}
		
		public string Name { get; set; }
		public int Locale { get; set; }
		public string Text { get; set; }
		public string Author { get; set; }

		//TODO: Добавить локализацию, примерно будет выглядеть так: есть статья, у нее есть только партишн и роу ключи, все остальное лежит в списке с объектами-локализациями.
	}

	public class LocalizedArticle : TableEntity
	{
		public LocalizedArticle()
		{
			Locales = new List<ArticleContent>();
		}

		public LocalizedArticle(string articleType, string name, int locale, string text, string author)
		{
			Locales = new List<ArticleContent>();
			PartitionKey = articleType;
			RowKey = DateTime.Now.ToString("yyyyMMddHHmmssffff");

			Locales.Add(new ArticleContent
				            {
					            Name = name,
					            Locale = locale,
					            Text = text,
					            Author = author
				            });
		}

		public LocalizedArticle AddLocale(string name, int locale, string text, string author)
		{
			if (Locales.All(l => l.Locale != locale))
				Locales.Add(new ArticleContent
					            {
						            Name = name,
						            Locale = locale,
						            Text = text,
						            Author = author
					            });
			return this;
		}

		public List<ArticleContent> Locales { get; set; }
	}

	public class ArticleContent
	{
		public string Name { get; set; }
		public int Locale { get; set; }
		public string Text { get; set; }
		public string Author { get; set; }
	}
}
