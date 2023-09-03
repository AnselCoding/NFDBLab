
using NFDBLab.Models;
using NFDBLab.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NFDBLab.Controllers
{
    public class ArticleController : ApiController
    {
        private ArticleService _service;
        public ArticleController()
        {
            _service = new ArticleService();
        }

        /// <summary>
        /// 文章有幾筆
        /// </summary>
        /// <returns>回應200與筆數</returns>
        // GET api/article/GetTotal
        [HttpGet]
        public IHttpActionResult GetTotal()
        {
            DataTable dt = _service.GetTotal();
            return this.Ok(dt);
        }

        /// <summary>
        /// 取得指定文章
        /// </summary>
        /// <param name="id">文章 id (article_ID)</param>
        /// <returns>找無該文章回應404，成功取得回應200與陣列該文章物件</returns>
        // GET api/article/GetSingle/1
        [HttpGet]
        public IHttpActionResult GetSingle(int id)
        {
            DataTable dt = _service.GetSingle(id);
            if (dt.Rows.Count == 0)
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return this.Ok(dt);
        }

        /// <summary>
        /// 取得所有文章內容
        /// </summary>
        /// <returns>回應200與陣列所有文章物件</returns>
        // GET api/article/GetTable
        [HttpGet]
        public IHttpActionResult GetTable()
        {
            DataTable dt = _service.GetTable();
            return this.Ok(dt);
        }

        /// <summary>
        /// 建立新文章
        /// </summary>
        /// <param name="article">新文章內容</param>
        /// <returns>成功建立回應204</returns>
        // POST api/article/Post
        [HttpPost]
        public IHttpActionResult Post(Article_Table article)
        {
            _service.Create(article);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NoContent));
        }

        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="article">要更新的文章物件</param>
        /// <returns>找無該文章回應404，成功更新回應204</returns>
        // PUT api/article/Put
        [HttpPut]
        public IHttpActionResult Put(Article_Table article)
        {
            if (IsItemExist(article.article_ID))
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            _service.Update(article);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NoContent));
        }

        /// <summary>
        /// 刪除文章
        /// </summary>
        /// <param name="id">文章 id (article_ID)</param>
        /// <returns>找無該文章回應404，成功刪除回應204</returns>
        // DELETE api/article/Delete/1
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            if (IsItemExist(id))
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            _service.Delete(id);
            return ResponseMessage(Request.CreateResponse(HttpStatusCode.NoContent));
        }
        private bool IsItemExist(int id)
        {
            return _service.GetSingle(id).Rows.Count == 0;
        }
    }
}
