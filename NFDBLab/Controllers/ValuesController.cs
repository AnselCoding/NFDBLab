using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NFDBLab.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values/Get
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/Get/5
        [HttpGet]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values/Post
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/Put/5
        [HttpPut]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/Delete/5
        [HttpDelete]
        public void Delete(int id)
        {
        }
    }
}
