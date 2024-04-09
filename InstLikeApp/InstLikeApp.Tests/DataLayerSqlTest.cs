using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InstLikeApp.Model;

namespace InstLikeApp.Tests
{
    [TestClass]
    public class DataLayerSqlTest
    {
        private const string _connectionString = "Data Source=vladimir-pc; Initial Catalog=InstLikeApp2; Integrated Security=True";

        // Add+Get+Delete metods checking for all entities-------------------------------------------->
        
        [TestMethod]
        public void ShouldAddGetDeleteUser()
        {
            //arrange
            var user = new User
            {
                UserName = "436576gaethrath" /*Guid.NewGuid().ToString()*/
            };
            //act
            var dataLayer = new DataLayer.Sql.DataLayer(_connectionString);
            var addUser = dataLayer.AddUser(user);

            var dataLayer1 = new DataLayer.Sql.DataLayer(_connectionString);
            var getUser = dataLayer1.GetUser(addUser.UserId);

            var dataLayer2 = new DataLayer.Sql.DataLayer(_connectionString);
            var isDeleted = dataLayer2.DeleteUser(getUser.UserId);
            //asserts
            Assert.AreEqual(addUser.UserId, getUser.UserId);
            Assert.AreEqual(addUser.UserName, getUser.UserName);
            Assert.IsNotNull(isDeleted);
        }
        
        [TestMethod]
        public void ShouldAddGetDeletePost()
        {
            //arrange
            var post = new Post
            {
                UserId = Guid.Parse("3c8fddae-8ebc-4cd4-9eb2-30ce678d6c23")/*Guid.NewGuid()*/,
                Picture = Guid.NewGuid().ToByteArray(),
                Date = DateTime.Now
            };
            //act
            var dataLayer = new DataLayer.Sql.DataLayer(_connectionString);
            var addPost = dataLayer.AddPost(post);
            var dataLayer1 = new DataLayer.Sql.DataLayer(_connectionString);
            var getPost = dataLayer1.GetPost(addPost.PostId);
            var dataLayer2 = new DataLayer.Sql.DataLayer(_connectionString);
            var isDeleted = dataLayer2.DeletePost(getPost.PostId);
            //asserts
            Assert.AreEqual(addPost.PostId, getPost.PostId);
            Assert.AreEqual(addPost.UserId, getPost.UserId);
            //Assert.AreEqual(addPost.Picture, getPost.Picture);
            //Assert.AreEqual(addPost.Date, getPost.Date);
            Assert.IsNotNull(isDeleted);
        }

        [TestMethod]
        public void ShouldAddGetDeleteComment()
        {
            //arrange
            var comment = new Comment
            {
                UserId = Guid.Parse("3c8fddae-8ebc-4cd4-9eb2-30ce678d6c23")/*Guid.NewGuid()*/,
                PostId = Guid.Parse("82243a65-d1e6-440c-a906-5dffb9cd653c")/*Guid.NewGuid()*/,
                Date = DateTime.Now,
                CommentText = "This is my comment"/*Guid.NewGuid().ToString()*/
            };
            //act
            var dataLayer = new DataLayer.Sql.DataLayer(_connectionString);
            var addComment = dataLayer.AddComment(comment);
            var dataLayer1 = new DataLayer.Sql.DataLayer(_connectionString);
            var getComment = dataLayer1.GetComment(addComment.CommentId);
            var dataLayer2 = new DataLayer.Sql.DataLayer(_connectionString);
            int isDeleted = dataLayer2.DeleteComment(getComment.CommentId);
            //asserts
            Assert.AreEqual(addComment.CommentId, getComment.CommentId);
            Assert.AreEqual(addComment.UserId, getComment.UserId);
            Assert.AreEqual(addComment.PostId, getComment.PostId);
            //Assert.AreEqual(addComment.Date, getComment.Date);
            Assert.AreEqual(addComment.CommentText, getComment.CommentText);
            Assert.IsNotNull(isDeleted);
        }

        [TestMethod]
        public void ShouldAddGetDeleteLike()
        {
            //arrange
            var like = new Like
            {
                UserId = Guid.Parse("3c8fddae-8ebc-4cd4-9eb2-30ce678d6c23")/*Guid.NewGuid()*/,
                PostId = Guid.Parse("82243a65-d1e6-440c-a906-5dffb9cd653c")/*Guid.NewGuid()*/,
            };
            //act
            var dataLayer = new DataLayer.Sql.DataLayer(_connectionString);
            var addLike = dataLayer.AddLike(like);
            var dataLayer1 = new DataLayer.Sql.DataLayer(_connectionString);
            var getLike = dataLayer1.GetLike(addLike.LikeId);
            var dataLayer2 = new DataLayer.Sql.DataLayer(_connectionString);
            int isDeleted = dataLayer2.DeleteLike(getLike.LikeId);
            //asserts
            Assert.AreEqual(addLike.LikeId, getLike.LikeId);
            Assert.AreEqual(addLike.UserId, getLike.UserId);
            Assert.AreEqual(addLike.PostId, getLike.PostId);
            Assert.IsNotNull(isDeleted);
        }

        [TestMethod]
        public void ShouldAddGetDeleteMark()
        {
            //arrange
            var mark = new Mark
            {
                UserId = Guid.Parse("3c8fddae-8ebc-4cd4-9eb2-30ce678d6c23")/*Guid.NewGuid()*/,
                PostId = Guid.Parse("82243a65-d1e6-440c-a906-5dffb9cd653c")/*Guid.NewGuid()*/,
            };
            //act
            var dataLayer = new DataLayer.Sql.DataLayer(_connectionString);
            var addMark = dataLayer.AddMark(mark);
            var dataLayer1 = new DataLayer.Sql.DataLayer(_connectionString);
            var getMark = dataLayer1.GetMark(addMark.MarkId);
            var dataLayer2 = new DataLayer.Sql.DataLayer(_connectionString);
            int isDeleted = dataLayer2.DeleteMark(getMark.MarkId);
            //asserts
            Assert.AreEqual(addMark.MarkId, getMark.MarkId);
            Assert.AreEqual(addMark.UserId, getMark.UserId);
            Assert.AreEqual(addMark.PostId, getMark.PostId);
            Assert.IsNotNull(isDeleted);
        }

        [TestMethod]
        public void ShouldAddGetDeleteHashtag()
        {
            //arrange
            var hashtag = new Hashtag
            {
                PostId = Guid.Parse("82243a65-d1e6-440c-a906-5dffb9cd653c")/*Guid.NewGuid()*/,
                HashtagText = "#hashtag"/*Guid.NewGuid().ToString()*/
            };
            //act
            var dataLayer = new DataLayer.Sql.DataLayer(_connectionString);
            var addHashtag = dataLayer.AddHashtag(hashtag);
            var dataLayer1 = new DataLayer.Sql.DataLayer(_connectionString);
            var getHashtag = dataLayer1.GetHashtag(addHashtag.HashtagId);
            var dataLayer2 = new DataLayer.Sql.DataLayer(_connectionString);
            int isDeleted = dataLayer2.DeleteHashtag(getHashtag.HashtagId);
            //asserts
            Assert.AreEqual(addHashtag.HashtagId, getHashtag.HashtagId);
            Assert.AreEqual(addHashtag.PostId, getHashtag.PostId);
            Assert.AreEqual(addHashtag.HashtagText, getHashtag.HashtagText);
            Assert.IsNotNull(isDeleted);
        }

        [TestMethod]
        public void ShouldAddGetDeleteReference()
        {
            //arrange
            var reference = new Reference
            {
                CommentId = Guid.Parse("6a4121a7-2b12-41e6-a408-8b13ef7d0b53")/*Guid.NewGuid()*/,
                UserId = Guid.Parse("3c8fddae-8ebc-4cd4-9eb2-30ce678d6c23")/*Guid.NewGuid()*/
            };
            //act
            var dataLayer = new DataLayer.Sql.DataLayer(_connectionString);
            var addReference = dataLayer.AddReference(reference);
            var dataLayer1 = new DataLayer.Sql.DataLayer(_connectionString);
            var getReference = dataLayer1.GetReference(addReference.ReferenceId);
            var dataLayer2 = new DataLayer.Sql.DataLayer(_connectionString);
            int isDeleted = dataLayer2.DeleteReference(getReference.ReferenceId);
            //asserts
            Assert.AreEqual(addReference.ReferenceId, getReference.ReferenceId);
            Assert.AreEqual(addReference.CommentId, getReference.CommentId);
            Assert.AreEqual(addReference.UserId, getReference.UserId);
            Assert.IsNotNull(isDeleted);
        }

        //----------------------------------------------------------------------------------<


        // Add metods checking for all entities-------------------------------------------->
        /* [TestMethod]
         public void ShouldAddUser()
         {
             //arrange
             var user = new User
             {
                 UserName = "21ffsrg"
             };
             //act
             var dataLayer = new DataLayer.Sql.DataLayer(_connectionString);
             var addUser = dataLayer.AddUser(user);
             //asserts
             Assert.AreEqual(addUser.UserId, user.UserId);
             Assert.AreEqual(addUser.UserName, user.UserName);
         }*/

        /* [TestMethod]
         public void ShouldAddPost()
         {
             //arrange
             var post = new Post
             {
                 UserId = Guid.Parse("f6f53096-4fe2-4def-a520-4879d09da80e"),
                 Picture = Guid.NewGuid().ToByteArray(),
                 Date = DateTime.Now
             };
             //act
             var dataLayer = new DataLayer.Sql.DataLayer(_connectionString);
             var addPost = dataLayer.AddPost(post);

             //asserts
             Assert.AreEqual(addPost.PostId, post.PostId);
             Assert.AreEqual(addPost.UserId, post.UserId);
         }*/

        /*  [TestMethod]
           public void ShouldAddComment()
           {
               //arrange
               var comment = new Comment
               {
                   UserId = Guid.Parse("f6f53096-4fe2-4def-a520-4879d09da80e"),
                   PostId = Guid.Parse("51f49bec-ec99-4b82-abc4-f6ad8a2d297a"),
                   Date = DateTime.Now,
                   CommentText = Guid.NewGuid().ToString()
               };
               //act
               var dataLayer = new DataLayer.Sql.DataLayer(_connectionString);
               var addComment = dataLayer.AddComment(comment);
               //asserts
               Assert.AreEqual(addComment.CommentId, comment.CommentId);
               Assert.AreEqual(addComment.UserId, comment.UserId);
               Assert.AreEqual(addComment.PostId, comment.PostId);
               Assert.AreEqual(addComment.CommentText, comment.CommentText);
           }*/

        /* [TestMethod]
         public void ShouldAddLike()
         {
             //arrange
             var like = new Like
             {
                 UserId = Guid.Parse("f6f53096-4fe2-4def-a520-4879d09da80e"),
                 PostId = Guid.Parse("51f49bec-ec99-4b82-abc4-f6ad8a2d297a"),
             };
             //act
             var dataLayer = new DataLayer.Sql.DataLayer(_connectionString);
             var addLike = dataLayer.AddLike(like);
             //asserts
             Assert.AreEqual(addLike.LikeId, like.LikeId);
             Assert.AreEqual(addLike.UserId, like.UserId);
             Assert.AreEqual(addLike.PostId, like.PostId);
         }*/

        /* [TestMethod]
         public void ShouldAddMark()
         {
             //arrange
             var mark = new Mark
             {
                 UserId = Guid.Parse("f6f53096-4fe2-4def-a520-4879d09da80e"),
                 PostId = Guid.Parse("51f49bec-ec99-4b82-abc4-f6ad8a2d297a"),
             };
             //act
             var dataLayer = new DataLayer.Sql.DataLayer(_connectionString);
             var addMark = dataLayer.AddMark(mark);
             //asserts
             Assert.AreEqual(addMark.MarkId, mark.MarkId);
             Assert.AreEqual(addMark.UserId, mark.UserId);
             Assert.AreEqual(addMark.PostId, mark.PostId);
         }*/

        /*[TestMethod]
        public void ShouldAddHashtag()
        {
            //arrange
            var hashtag = new Hashtag
            {
                PostId = Guid.NewGuid(),
                HashtagText = Guid.NewGuid().ToString()
            };
            //act
            var dataLayer = new DataLayer.Sql.DataLayer(_connectionString);
            var addHashtag = dataLayer.AddHashtag(hashtag);
            //asserts
            Assert.AreEqual(addHashtag.HashtagId, hashtag.HashtagId);
            Assert.AreEqual(addHashtag.PostId, hashtag.PostId);
            Assert.AreEqual(addHashtag.HashtagText, hashtag.HashtagText);
        }*/

        /* [TestMethod]
         public void ShouldAddReference()
         {
             //arrange
             var reference = new Reference
             {
                 CommentId = Guid.NewGuid(),
                 UserId = Guid.NewGuid()
             };
             //act
             var dataLayer = new DataLayer.Sql.DataLayer(_connectionString);
             var addReference = dataLayer.AddReference(reference);
             //asserts
             Assert.AreEqual(addReference.ReferenceId, reference.ReferenceId);
             Assert.AreEqual(addReference.CommentId, reference.CommentId);
             Assert.AreEqual(addReference.UserId, reference.UserId);
         }*/
        //---------------------------------------------------------------------------------<
    }
}
