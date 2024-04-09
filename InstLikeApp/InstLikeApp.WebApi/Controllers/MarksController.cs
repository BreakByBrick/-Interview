using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InstLikeApp.Model;

namespace InstLikeApp.WebApi.Controllers
{
    public class MarksController : ApiController
    {
        private const string ConnectionString = "Data Source=vladimir-pc; Initial Catalog=InstLikeApp2; Integrated Security=True";
        private readonly IDataLayer _dataLayer;

        public MarksController()
        {
            _dataLayer = new DataLayer.Sql.DataLayer(ConnectionString);
        }

        [HttpPost]
        public Mark AddMark(Mark mark)
        {
            return _dataLayer.AddMark(mark);
        }

        [HttpGet]
        [Route("api/marks/{id}")]
        public Mark GetReference(Guid id)
        {
            return _dataLayer.GetMark(id);
        }

        [HttpDelete]
        [Route("api/marks/{id}")]
        public int DeleteMark(Guid id)
        {
            return _dataLayer.DeleteMark(id);
        }
    }
}
