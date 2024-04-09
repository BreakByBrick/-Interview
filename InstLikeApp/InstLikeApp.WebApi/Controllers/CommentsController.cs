using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InstLikeApp.Model;
using NLog;

namespace InstLikeApp.WebApi.Controllers
{
    public class CommentsController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private const string ConnectionString = "Data Source=vladimir-pc; Initial Catalog=InstLikeApp2; Integrated Security=True";
        private readonly IDataLayer _dataLayer;

        public CommentsController()
        {
            _dataLayer = new DataLayer.Sql.DataLayer(ConnectionString);
        }

        [HttpPost]
        [Route("api/comments/AddComment/")]
        public Comment AddComment(Comment comment)
        {
            return _dataLayer.AddComment(comment);
        }

        [HttpGet]
        [Route("api/comments/GetCommentById/{id}")]
        public Comment GetComment(Guid id)
        {
            return _dataLayer.GetComment(id);
        }

        [HttpGet]
        [Route("api/comments/GetCommentsToPost/{postId}")]
        public Comment[] GetCommentsToPost(Guid postId)
        {
            return _dataLayer.GetCommentsToPost(postId);
        }

        [HttpGet]
        [Route("api/comments/GetCommentsOfUser/{userId}")]
        public Comment[] GetCommentsOfUser(Guid userId)
        {
            return _dataLayer.GetCommentsOfUser(userId);
        }

        [HttpDelete]
        [Route("api/comments/{id}")]
        public int DeleteComment(Guid id)
        {
            return _dataLayer.DeleteComment(id);
        }
    }
}
