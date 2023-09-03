using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NFDBLab.Models
{
    public class Article_Table
    {
        public int article_ID { get; set; }

        public string article_Board_C { get; set; }

        public int article_Author { get; set; }

        public string article_Title { get; set; }

        public int article_ReplyCount { get; set; }

        public int article_LikesCount { get; set; }

        public DateTime article_BuildTime { get; set; }

        public DateTime article_LastEdit { get; set; }

        public string article_Content { get; set; }

    }
}