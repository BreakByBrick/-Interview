using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InstLikeApp.Model;

namespace InstLikeApp.WebApi.Controllers
{
    public class LikesController : ApiController
    {
        private const string ConnectionString = "Data Source=vladimir-pc; Initial Catalog=InstLikeApp2; Integrated Security=True";
        private readonly IDataLayer _dataLayer;

        public LikesController()
        {
            _dataLayer = new DataLayer.Sql.DataLayer(ConnectionString);
        }

        [HttpPost]
        public Like AddLike(Like like)
        {
            return _dataLayer.AddLike(like);
        }

        [HttpGet]
        [Route("api/likes/{id}")]
        public Like GetLike(Guid id)
        {
            return _dataLayer.GetLike(id);
        }

        [HttpDelete]
        [Route("api/likes/{id}")]
        public int DeleteLike(Guid id)
        {
            return _dataLayer.DeleteLike(id);
        }
    }
}
