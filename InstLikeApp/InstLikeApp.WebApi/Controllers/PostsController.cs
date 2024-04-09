using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InstLikeApp.Model;

namespace InstLikeApp.WebApi.Controllers
{
    public class PostsController : ApiController
    {
        private const string ConnectionString = "Data Source=vladimir-pc; Initial Catalog=InstLikeApp2; Integrated Security=True";
        private readonly IDataLayer _dataLayer;

        public PostsController()
        {
            _dataLayer = new DataLayer.Sql.DataLayer(ConnectionString);
        }

        [HttpPost]
        [Route("api/posts/AddPost/")]
        public Post AddPost(Post post)
        {
            return _dataLayer.AddPost(post);
        }

        [HttpGet]
        [Route("api/posts/GetPostById/{id}")]
        public Post GetPost(Guid id)
        {
            return _dataLayer.GetPost(id);
        }

        [HttpGet]
        [Route("api/posts/GetAllPosts/")]
        public Post[] GetAllPosts()
        {
            return _dataLayer.GetAllPosts();
        }

        [HttpGet]
        [Route("api/posts/GetPostsOfUser/{userId}")]
        public Post[] GetPostsOfUser(Guid userId)
        {
            return _dataLayer.GetPostsOfUser(userId);
        }

        [HttpDelete]
        [Route("api/posts/DeletePostById/{id}")]
        public int DeletePost(Guid id)
        {
            return _dataLayer.DeletePost(id);
        }
    }
}
