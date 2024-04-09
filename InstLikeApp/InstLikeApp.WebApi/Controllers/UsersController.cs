using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InstLikeApp.Model;

namespace InstLikeApp.WebApi.Controllers
{
    public class UsersController : ApiController
    {
        private const string ConnectionString = "Data Source=vladimir-pc; Initial Catalog=InstLikeApp2; Integrated Security=True";
        private readonly IDataLayer _dataLayer;

        public UsersController()
        {
            _dataLayer = new DataLayer.Sql.DataLayer(ConnectionString);
        }

        [HttpPost]
        [Route("api/users/AddUser/")]
        public User AddUser([FromBody]User user)
        {
            return _dataLayer.AddUser(user);
        }

        [HttpGet]
        [Route("api/users/{id}")]
        public User GetUser(Guid id)
        {
            return _dataLayer.GetUser(id);
        }

        [HttpGet]
        [Route("api/users/GetUserByName/{userName}")]
        public User GetUserByName(string userName)
        {
            return _dataLayer.GetUserByName(userName);
        }

        [HttpDelete]
        [Route("api/users/{id}")]
        public int DeleteUser(Guid id)
        {
            return _dataLayer.DeleteUser(id);
        }
    }
}
