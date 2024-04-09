using InstLikeApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace InstLikeApp.Client
{
    class HttpClientWrapper
    {
        private readonly string _connectionString;
        private readonly HttpClient _client;

        public HttpClientWrapper(string connectionString)
        {
            _connectionString = connectionString;
            _client = new HttpClient
            {
                BaseAddress = new Uri(connectionString)
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public User GetUserById(Guid id)
        {
            HttpResponseMessage response = _client.GetAsync(string.Format("{0}/api/users/{1}", _connectionString, id)).Result;
            var result = response.Content.ReadAsAsync<User>().Result;
            return result;
        }

        public User GetUserByName(string userName)
        {
            HttpResponseMessage response = _client.GetAsync(string.Format("{0}api/users/GetUserByName/{1}", _connectionString, userName)).Result;
            var result = response.Content.ReadAsAsync<User>().Result;
            return result;
        }

        public Post AddPost(Post post)
        {
            MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();
            var content = new ObjectContent<Post>(post, jsonFormatter);
            HttpResponseMessage response = _client.PostAsync(string.Format("{0}api/posts/AddPost/", _connectionString), content).Result;
            var result = response.Content.ReadAsAsync<Post>().Result;
            return result;
        }

        public User AddUser(User user)
        {
            MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();
            var content = new ObjectContent<User>(user, jsonFormatter);
            HttpResponseMessage response = _client.PostAsync(string.Format("{0}api/users/AddUser/ ", _connectionString), content).Result;
            var result = response.Content.ReadAsAsync<User>().Result;
            return result;
        }

        public Comment AddComment(Comment comment)
        {
            MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();
            var content = new ObjectContent<Comment>(comment, jsonFormatter);
            HttpResponseMessage response = _client.PostAsync(string.Format("{0}api/comments/AddComment/ ", _connectionString), content).Result;
            var result = response.Content.ReadAsAsync<Comment>().Result;
            return result;
        }

        public Post[] GetPostsOfUser(Guid userId)
        {
            HttpResponseMessage response = _client.GetAsync(string.Format("{0}api/posts/GetPostsOfUser/{1}", _connectionString, userId)).Result;
            var result = response.Content.ReadAsAsync<Post[]>().Result;
            return result;
        }

        public Comment[] GetCommentsToPost(Guid postId)
        {
            HttpResponseMessage response = _client.GetAsync(string.Format("{0}api/comments/GetCommentsToPost/{1}", _connectionString, postId)).Result;
            var result = response.Content.ReadAsAsync<Comment[]>().Result;
            return result;
        }

        public Post[] GetAllPosts()
        {
            HttpResponseMessage response = _client.GetAsync(string.Format("{0}api/posts/GetAllPosts/", _connectionString)).Result;
            var result = response.Content.ReadAsAsync<Post[]>().Result;
            return result;
        }
    }
}
