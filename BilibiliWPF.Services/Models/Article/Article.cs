using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiliWpf.Services.Models.Article
{
    public class ArticleResponse
    {
        public int count { get; set; }
        public List<Article> item { get; set; }
    }

    public class Article
    {
    }
}
