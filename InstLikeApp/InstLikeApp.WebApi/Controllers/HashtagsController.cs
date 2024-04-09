using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InstLikeApp.Model;

namespace InstLikeApp.WebApi.Controllers
{
    public class HashtagsController : ApiController
    {
        private const string ConnectionString = "Data Source=vladimir-pc; Initial Catalog=InstLikeApp2; Integrated Security=True";
        private readonly IDataLayer _dataLayer;

        public HashtagsController()
        {
            _dataLayer = new DataLayer.Sql.DataLayer(ConnectionString);
        }

        [HttpPost]
        public Hashtag AddHashtag(Hashtag hashtag)
        {
            return _dataLayer.AddHashtag(hashtag);
        }

        [HttpGet]
        [Route("api/hashtags/{id}")]
        public Hashtag GetHashtag(Guid id)
        {
            return _dataLayer.GetHashtag(id);
        }

        [HttpDelete]
        [Route("api/hashtags/{id}")]
        public int DeleteHashtag(Guid id)
        {
            return _dataLayer.DeleteHashtag(id);
        }
    }
}
