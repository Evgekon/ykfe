using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace kyfelib
{
	public class Article : TableEntity
	{
		public Article()
		{
			RowKey = DateTime.UtcNow.ToString("yyyyMMddHHmmssffff");
		}
	}
}
