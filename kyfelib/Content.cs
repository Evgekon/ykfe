using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace kyfelib
{
	public class Content : TableEntity
	{
		public Content()
		{
			RowKey = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
		}

		public Content(ContentType type)
		{
			RowKey = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
			PartitionKey = Enum.GetName(typeof (ContentType), type);
		}
	}

	public class ContentLocale : TableEntity
	{
		public ContentLocale()
		{
			RowKey = Guid.NewGuid().ToString();
		}

		public ContentLocale(Locale locale)
		{
			RowKey = Guid.NewGuid().ToString();
			PartitionKey = Enum.GetName(typeof(Locale), locale);
		}

		public string ContentId { get; set; }
		public string Name { get; set; }
		public string Text { get; set; }
		public string Author { get; set; }
		public bool IsDraft { get; set; }
		public string Tags { get; set; }
	}

	public class ContentModel
	{
		public ContentModel()
		{
			Locales = new List<ContentModelLocale>();
		}

		public string Id { get; set; }
		public DateTime Date { get; set; }
		public ContentType Type { get; set; }
		public List<ContentModelLocale> Locales { get; set; } 
	}

	public class ContentModelLocale
	{
		public ContentModelLocale()
		{
			Tags = new List<string>();
		}

		public string Id { get; set; } //Guid
		public Locale Locale { get; set; }
		public string Name { get; set; }
		public string Text { get; set; }
		public string Author { get; set; }
		public bool IsDraft { get; set; }
		public List<string> Tags { get; set; }
	}
}
