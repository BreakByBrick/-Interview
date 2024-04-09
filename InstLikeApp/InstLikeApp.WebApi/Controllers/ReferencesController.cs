using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InstLikeApp.Model;

namespace InstLikeApp.WebApi.Controllers
{
    public class ReferencesController : ApiController
    {
        private const string ConnectionString = "Data Source=vladimir-pc; Initial Catalog=InstLikeApp2; Integrated Security=True";
        private readonly IDataLayer _dataLayer;

        public ReferencesController()
        {
            _dataLayer = new DataLayer.Sql.DataLayer(ConnectionString);
        }

        [HttpPost]
        public Reference AddReference(Reference reference)
        {
            return _dataLayer.AddReference(reference);
        }

        [HttpGet]
        [Route("api/references_t/{id}")]
        public Reference GetReference(Guid id)
        {
            return _dataLayer.GetReference(id);
        }

        [HttpDelete]
        [Route("api/references_t/{id}")]
        public int DeleteReference(Guid id)
        {
            return _dataLayer.DeleteReference(id);
        }
    }
}
