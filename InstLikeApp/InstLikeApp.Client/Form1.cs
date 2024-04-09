using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InstLikeApp.Model;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.IO;

namespace InstLikeApp.Client
{
    public partial class Form1 : Form
    {

        private readonly HttpClientWrapper _httpClient = new HttpClientWrapper("http://localhost:57171/");

        public Guid _currentPostId;
        public Post[] _posts;
        public Comment[] _comments;
        public int _postIndex;
        public string _postsShow;

        public void changeButtonStatus(string message)
        {
            switch (message)
            {
                case "FormInitialized":
                    buttonAddUser.Enabled = false;
                    buttonAddPost.Enabled = false;
                    buttonShowUserPosts.Enabled = false;
                    buttonPrevPost.Enabled = false;
                    buttonAddComment.Enabled = false;
                    if (_posts.Length == 0)
                    {
                        buttonNextPost.Enabled = false;
                        buttonAddComment.Enabled = false;
                        buttonShowLastPosts.Enabled = false;
                    }
                    else if(_posts.Length == 1)
                    {
                        buttonNextPost.Enabled = false;
                        buttonShowLastPosts.Enabled = true;
                    }
                    else
                    {
                        buttonNextPost.Enabled = true;
                        buttonShowLastPosts.Enabled = true;
                    }
                            
                    break;

                case "UserAdded":
                    buttonShowUserPosts.Enabled = true;
                    buttonAddComment.Enabled = true;
                    buttonAddPost.Enabled = true;
                    if(_posts.Length != 0)
                        buttonAddComment.Enabled = true;
                    else
                        buttonAddComment.Enabled = false;
                    buttonAddUser.Enabled = false;
                    break;

                case "UserNameChanged":
                    var user = _httpClient.GetUserByName(textBoxUserName.Text);
                    if (user.UserId.ToString() == "00000000-0000-0000-0000-000000000000")
                    {
                        if (textBoxUserName.Text != "")
                            buttonAddUser.Enabled = true;
                        else
                            buttonAddUser.Enabled = false;
                        buttonAddPost.Enabled = false;
                        buttonShowUserPosts.Enabled = false;
                        buttonAddComment.Enabled = false;
                    }
                    else
                    {
                        buttonAddUser.Enabled = false;
                        buttonAddPost.Enabled = true;
                        _posts = _httpClient.GetPostsOfUser(user.UserId);
                        if (_posts.Length != 0)
                            buttonShowUserPosts.Enabled = true;
                        else
                            buttonShowUserPosts.Enabled = false;
                        buttonAddComment.Enabled = true;
                    }
                    break;

                case "PostAdded":
                    buttonShowUserPosts.Enabled = true;
                    buttonShowLastPosts.Enabled = true;
                    buttonAddComment.Enabled = true;
                    buttonPrevPost.Enabled = false;
                    buttonNextPost.Enabled = false;
                    break;

                case "PrevPostButtonPressed":
                    buttonNextPost.Enabled = true;
                    if (_postIndex == 0)
                        buttonPrevPost.Enabled = false;
                    break;

                case "NextPostButtonPressed":
                    buttonPrevPost.Enabled = true;
                    if (_postIndex == _posts.Length - 1)
                        buttonNextPost.Enabled = false;
                    break;

                case "ShowAllPostsButtonPressed":
                    buttonPrevPost.Enabled = false;
                    if (_posts.Length == 0)
                    {
                        buttonNextPost.Enabled = false;
                        buttonAddComment.Enabled = false;
                        buttonShowLastPosts.Enabled = false;
                    }
                    else if (_posts.Length == 1)
                    {
                        buttonNextPost.Enabled = false;
                        buttonShowLastPosts.Enabled = true;
                    }
                    else
                    {
                        buttonNextPost.Enabled = true;
                        buttonShowLastPosts.Enabled = true;
                    }
                    break;

                case "ShowUserPostsButtonPressed":
                    buttonPrevPost.Enabled = false;
                    if (_posts.Length == 0)
                    {
                        buttonNextPost.Enabled = false;
                        buttonAddComment.Enabled = false;
                        buttonShowLastPosts.Enabled = false;
                    }
                    else if (_posts.Length == 1)
                    {
                        buttonNextPost.Enabled = false;
                        buttonShowLastPosts.Enabled = true;
                    }
                    else
                    {
                        buttonNextPost.Enabled = true;
                        buttonShowLastPosts.Enabled = true;
                    }
                    break;

                default:
                    
                    break;
            }      
        }

        public Form1()
        {
            InitializeComponent();

            _postIndex = 0;
            _posts = _httpClient.GetAllPosts();

            changeButtonStatus("FormInitialized");

            if(_posts.Length != 0)
            {
                MemoryStream stream = new MemoryStream(_posts[_postIndex].Picture);
                pictureBox1.Image = Image.FromStream(stream);

                var user = _httpClient.GetUserById(_posts[_postIndex].UserId);
                labelPost.Text = "Posted by:" + user.UserName + " (" + _posts[_postIndex].Date.ToString() + ")";

                _comments = _httpClient.GetCommentsToPost(_posts[_postIndex].PostId);

                textBoxCommentsText.Clear();
                for (int i = 0; i < _comments.Length; i++)
                {
                    user = _httpClient.GetUserById(_comments[i].UserId);
                    textBoxCommentsText.Text = textBoxCommentsText.Text + Environment.NewLine + "(" + _comments[i].Date.ToString() + ") " + user.UserName + ":" + Environment.NewLine + _comments[i].CommentText + Environment.NewLine;
                }

                _currentPostId = _posts[_postIndex].PostId;
                _postsShow = "all";
            }
        }

        private void buttonAddPost_Click(object sender, EventArgs e)
        {
            changeButtonStatus("PostAdded");

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";
            
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Image picture = Image.FromFile(dialog.FileName);
                var user = _httpClient.GetUserByName(textBoxUserName.Text);
                Post post = new Post
                {
                    PostId = Guid.NewGuid(),
                    UserId = user.UserId,
                    Picture = File.ReadAllBytes(dialog.FileName), /*Guid.NewGuid().ToByteArray(),*/
                    Date = DateTime.Now
                };

                pictureBox1.Image = picture;
                labelPost.Text = "Posted by:" + user.UserName + " (" + _posts[_postIndex].Date.ToString() + ")";
                textBoxCommentsText.Clear();

                _currentPostId = post.PostId;

                var result = _httpClient.AddPost(post);
                
            }
        }

        private void buttonAddUser_Click(object sender, EventArgs e)
        {
            User user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = textBoxUserName.Text
            };

            var result = _httpClient.AddUser(user);
            changeButtonStatus("UserAdded");
        }

        private void textBoxUserName_textChanged(object sender, EventArgs e)
        {
            changeButtonStatus("UserNameChanged");       
        }

        private void buttonAddComment_Click(object sender, EventArgs e)
        {
            User user = _httpClient.GetUserByName(textBoxUserName.Text);
            Comment comment = new Comment
            {
                CommentId = Guid.NewGuid(),
                UserId = user.UserId,
                PostId = _currentPostId,
                Date = DateTime.Now,
                CommentText = textBoxNewComment.Text
            };
            var result = _httpClient.AddComment(comment);

            textBoxCommentsText.Text = textBoxCommentsText.Text + Environment.NewLine;
            textBoxCommentsText.Text = textBoxCommentsText.Text + "(" + comment.Date.ToString() + ") " + user.UserName + ":" + Environment.NewLine + comment.CommentText + Environment.NewLine;
            textBoxNewComment.Clear();
        }

        private void buttonShowUserPosts_Click(object sender, EventArgs e)
        {
            var user = _httpClient.GetUserByName(textBoxUserName.Text);
            _posts = _httpClient.GetPostsOfUser(user.UserId);
            _postIndex = 0;
            changeButtonStatus("ShowUserPostsButtonPressed");

            if (_posts.Length != 0)
            {
                MemoryStream stream = new MemoryStream(_posts[_postIndex].Picture);
                pictureBox1.Image = Image.FromStream(stream);

                user = _httpClient.GetUserById(_posts[_postIndex].UserId);
                labelPost.Text = "Posted by:" + user.UserName + " (" + _posts[_postIndex].Date.ToString() + ")";

                _comments = _httpClient.GetCommentsToPost(_posts[_postIndex].PostId);

                textBoxCommentsText.Clear();
                for (int i = 0; i < _comments.Length; i++)
                {
                    var userName = _httpClient.GetUserById(_comments[i].UserId).UserName;
                    textBoxCommentsText.Text = textBoxCommentsText.Text + Environment.NewLine + "(" + _comments[i].Date.ToString() + ") " + userName + ":" + Environment.NewLine + _comments[i].CommentText + Environment.NewLine;
                }
                _currentPostId = _posts[_postIndex].PostId;
            }
            _postsShow = "user";
        }

        private void buttonNextPost_Click(object sender, EventArgs e)
        {
            
            _postIndex++;
            if (_postsShow == "all")
                _posts = _httpClient.GetAllPosts();
            else
                _posts = _httpClient.GetPostsOfUser(_httpClient.GetUserByName(textBoxUserName.Text).UserId);

            changeButtonStatus("NextPostButtonPressed");

            if (_posts.Length != 0)
            {
                MemoryStream stream = new MemoryStream(_posts[_postIndex].Picture);
                pictureBox1.Image = Image.FromStream(stream);

                var user = _httpClient.GetUserById(_posts[_postIndex].UserId);
                labelPost.Text = "Posted by:" + user.UserName + " (" + _posts[_postIndex].Date.ToString() + ")";

                _comments = _httpClient.GetCommentsToPost(_posts[_postIndex].PostId);

                textBoxCommentsText.Clear();
                for (int i = 0; i < _comments.Length; i++)
                {
                    user = _httpClient.GetUserById(_comments[i].UserId);
                    textBoxCommentsText.Text = textBoxCommentsText.Text + Environment.NewLine + "(" + _comments[i].Date.ToString() + ") " + user.UserName + ":" + Environment.NewLine + _comments[i].CommentText + Environment.NewLine;
                }
                _currentPostId = _posts[_postIndex].PostId;
            }
        }

        private void buttonPrevPost_Click(object sender, EventArgs e)
        {
            
            _postIndex--;
            if (_postsShow == "all")
                _posts = _httpClient.GetAllPosts();
            else
                _posts = _httpClient.GetPostsOfUser(_httpClient.GetUserByName(textBoxUserName.Text).UserId);

            changeButtonStatus("PrevPostButtonPressed");

            if (_posts.Length != 0)
            {
                MemoryStream stream = new MemoryStream(_posts[_postIndex].Picture);
                pictureBox1.Image = Image.FromStream(stream);

                var user = _httpClient.GetUserById(_posts[_postIndex].UserId);
                labelPost.Text = "Posted by:" + user.UserName + " (" + _posts[_postIndex].Date.ToString() + ")";

                _comments = _httpClient.GetCommentsToPost(_posts[_postIndex].PostId);

                textBoxCommentsText.Clear();
                for (int i = 0; i < _comments.Length; i++)
                {
                    user = _httpClient.GetUserById(_comments[i].UserId);
                    textBoxCommentsText.Text = textBoxCommentsText.Text + Environment.NewLine + "(" + _comments[i].Date.ToString() + ") " + user.UserName + ":" + Environment.NewLine + _comments[i].CommentText + Environment.NewLine;
                }
                _currentPostId = _posts[_postIndex].PostId;
            }
        }

        private void buttonShowLastPosts_Click(object sender, EventArgs e)
        {
            _postIndex = 0;
            _posts = _httpClient.GetAllPosts();

            changeButtonStatus("ShowAllPostsButtonPressed");
            
            if (_posts.Length != 0)
            {
                MemoryStream stream = new MemoryStream(_posts[_postIndex].Picture);
                pictureBox1.Image = Image.FromStream(stream);

                var user = _httpClient.GetUserById(_posts[_postIndex].UserId);
                labelPost.Text = "Posted by:" + user.UserName + " (" + _posts[_postIndex].Date.ToString() + ")";

                _comments = _httpClient.GetCommentsToPost(_posts[_postIndex].PostId);

                textBoxCommentsText.Clear();
                for (int i = 0; i < _comments.Length; i++)
                {
                    user = _httpClient.GetUserById(_comments[i].UserId);
                    textBoxCommentsText.Text = textBoxCommentsText.Text + Environment.NewLine + "(" + _comments[i].Date.ToString() + ") " + user.UserName + ":" + Environment.NewLine + _comments[i].CommentText + Environment.NewLine;
                }

                _currentPostId = _posts[_postIndex].PostId;
                _postsShow = "all";
            }
        }
    }
}
