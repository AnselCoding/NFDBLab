using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NFDBLab.Models
{
    public class ArticleLike_Table
    {
        public int articleLike_ID { get; set; }

        public int article_ID { get; set; }

        public int member_ID { get; set; }

    }

}