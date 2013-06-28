using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using kyfelib;

namespace kyfewww.Models
{
	public class ArticleModel
	{
		public string Id { get; set; }
		public ArticleType Type { get; set; }
		public List<ArticleContent> Contents { get; set; }
		public List<Tag> Tags { get; set; } 
	}
}