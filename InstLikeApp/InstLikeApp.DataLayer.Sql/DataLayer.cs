using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstLikeApp.Model;
using System.Data.SqlClient;
using NLog;
using NLog.Config;
using System.Diagnostics;

namespace InstLikeApp.DataLayer.Sql
{
    public class DataLayer : IDataLayer
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //private readonly Logger _instance = LogManager.GetCurrentClassLogger();

        private readonly string _connectionString;

        public DataLayer(string connectionString)
        {
            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString));

            _connectionString = connectionString;
        }

        //Writing into BD-------------------------------------------------------------->
        public Comment AddComment(Comment comment)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    //Exceptions----------------------------------------------------->
                    if (comment.CommentText.Length > 500)
                        throw new ArgumentException("Comment is too long.");

                    command.CommandText = "SELECT TOP (1) UserId FROM Users WHERE UserId = @usId";
                    command.Parameters.AddWithValue("@usId", comment.UserId);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        if (!reader.HasRows)
                            throw new ArgumentException("User with that ID not found.");
                    }                

                    command.CommandText = "SELECT TOP (1) PostId FROM Posts WHERE PostId = @pId";
                    command.Parameters.AddWithValue("@pId", comment.PostId);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        if (!reader.HasRows)
                            throw new ArgumentException("Post with that ID not found.");
                    }      
                    //----------------------------------------------------------------<

                    comment.CommentId = Guid.NewGuid();
                    command.CommandText = "INSERT INTO Comments (CommentId, UserId, PostId, Date, CommentText) VALUES (@commentId, @userId, @postId, @date, @commentText)";
                    command.Parameters.AddWithValue("@commentId", comment.CommentId);
                    command.Parameters.AddWithValue("@userId", comment.UserId);
                    command.Parameters.AddWithValue("@postId", comment.PostId);
                    command.Parameters.AddWithValue("@date", comment.Date);
                    command.Parameters.AddWithValue("@commentText", comment.CommentText);
                    command.ExecuteNonQuery();
                    logger.Trace("log "); //logger.Trace("Logging");
                    return comment;
                }
            }
        }

        public Hashtag AddHashtag(Hashtag hashtag)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    //Exceptions----------------------------------------------------->
                    if (hashtag.HashtagText[0] != '#')
                        throw new ArgumentException("Hashtag must have # in beginning.");

                    string wrongSymbols = "!?/.,<>%^&$*+- []{}()'`~@#\"";
                    for (int i = 0; i < wrongSymbols.Length; i++)
                        if (hashtag.HashtagText.Substring(1,hashtag.HashtagText.Length-1).Contains(wrongSymbols[i]))
                            throw new ArgumentException("Hastag text contains wrong symbols.");

                    if (hashtag.HashtagText.Length > 30)
                        throw new ArgumentException("Hashtag is too long.");

                    command.CommandText = "SELECT TOP (1) HashtagId FROM Hashtags WHERE HashtagText = @ht";
                    command.Parameters.AddWithValue("@ht", hashtag.HashtagText);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        if (reader.HasRows)
                            throw new ArgumentException("That hashtag already exists.");
                    }

                    command.CommandText = "SELECT TOP (1) PostId FROM Posts WHERE PostId = @pId";
                    command.Parameters.AddWithValue("@pId", hashtag.PostId);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        if (!reader.HasRows)
                            throw new ArgumentException("Post with that ID not found.");
                    }
                    //----------------------------------------------------------------<

                    hashtag.HashtagId = Guid.NewGuid();
                    command.CommandText = "INSERT INTO Hashtags (HashtagId, PostId, HashtagText) VALUES (@hashtagId, @postId, @hashtagText)";
                    command.Parameters.AddWithValue("@hashtagId", hashtag.HashtagId);
                    command.Parameters.AddWithValue("@postId", hashtag.PostId);
                    command.Parameters.AddWithValue("@hashtagText", hashtag.HashtagText);
                    command.ExecuteNonQuery();
                    logger.Trace("log ");//logger.Trace("Logging");
                    return hashtag;
                }
            }
        }

        public Like AddLike(Like like)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    //Exceptions----------------------------------------------------->
                    command.CommandText = "SELECT TOP (1) LikeId FROM Likes WHERE (PostId = @pId) AND (UserId = @uId) ";
                    command.Parameters.AddWithValue("@pId", like.PostId);
                    command.Parameters.AddWithValue("@uId", like.UserId);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        if (reader.HasRows)
                            throw new ArgumentException("Like for this post already exists.");
                    }

                    command.CommandText = "SELECT TOP (1) UserId FROM Users WHERE UserId = @usId";
                    command.Parameters.AddWithValue("@usId", like.UserId);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        if (!reader.HasRows)
                            throw new ArgumentException("User with that ID not found.");
                    }

                    command.CommandText = "SELECT TOP (1) PostId FROM Posts WHERE PostId = @posId";
                    command.Parameters.AddWithValue("@posId", like.PostId);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        if (!reader.HasRows)
                            throw new ArgumentException("Post with that ID not found.");
                    }
                    //----------------------------------------------------------------<

                    like.LikeId = Guid.NewGuid();
                    command.CommandText = "INSERT INTO Likes (LikeId,UserId,PostId) VALUES (@likeId, @userId, @postId)";
                    command.Parameters.AddWithValue("@likeId", like.LikeId);
                    command.Parameters.AddWithValue("@userId", like.UserId);
                    command.Parameters.AddWithValue("@postId", like.PostId);
                    command.ExecuteNonQuery();
                    logger.Trace("log ");//logger.Trace("Logging");
                    return like;
                }
            }
        }

        public Mark AddMark(Mark mark)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    //Exceptions----------------------------------------------------->
                    command.CommandText = "SELECT TOP (1) MarkId FROM Marks WHERE (PostId = @pId) AND (UserId = @uId) ";
                    command.Parameters.AddWithValue("@pId", mark.PostId);
                    command.Parameters.AddWithValue("@uId", mark.UserId);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        if (reader.HasRows)
                            throw new ArgumentException("Like for this post already exists.");
                    }

                    command.CommandText = "SELECT TOP (1) UserId FROM Users WHERE UserId = @usId";
                    command.Parameters.AddWithValue("@usId", mark.UserId);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        if (!reader.HasRows)
                            throw new ArgumentException("User with that ID not found.");
                    }

                    command.CommandText = "SELECT TOP (1) PostId FROM Posts WHERE PostId = @posId";
                    command.Parameters.AddWithValue("@posId", mark.PostId);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        if (!reader.HasRows)
                            throw new ArgumentException("Post with that ID not found.");
                    }
                    //---------------------------------------------------------------<

                    mark.MarkId = Guid.NewGuid();
                    command.CommandText = "INSERT INTO Marks (MarkId, PostId, UserId) VALUES (@markId, @postId, @userId)";
                    command.Parameters.AddWithValue("@markId", mark.MarkId);
                    command.Parameters.AddWithValue("@postId", mark.PostId);
                    command.Parameters.AddWithValue("@userId", mark.UserId);
                    command.ExecuteNonQuery();
                    logger.Trace("log ");//logger.Trace("Logging");
                    return mark;
                }
            }
        }

        public Post AddPost(Post post)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    //Exceptions----------------------------------------------------->
                    if (post.Picture.Length > 209715200)
                        throw new ArgumentException("Size of photo too big.");

                    command.CommandText = "SELECT TOP (1) UserId FROM Users WHERE UserId = @uId";
                    command.Parameters.AddWithValue("@uId", post.UserId);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        if (!reader.HasRows)
                            throw new ArgumentException("User with that ID not found.");
                    }
                    //---------------------------------------------------------------<

                    post.PostId = Guid.NewGuid();
                    command.CommandText = "INSERT INTO Posts (PostId, UserId, Picture, Date) VALUES (@postId, @userId, @picture, @date)";
                    command.Parameters.AddWithValue("@postId", post.PostId);
                    command.Parameters.AddWithValue("@userId", post.UserId);
                    command.Parameters.AddWithValue("@picture", post.Picture);
                    command.Parameters.AddWithValue("@date", post.Date);
                    command.ExecuteNonQuery();
                    logger.Trace("log ");//logger.Trace("Logging");
                    return post;
                }
            }
        }

        public Reference AddReference(Reference reference)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    //Exceptions----------------------------------------------------->
                    command.CommandText = "SELECT TOP (1) CommentId FROM Comments WHERE CommentId = @cId";
                    command.Parameters.AddWithValue("@cId", reference.CommentId);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        if (!reader.HasRows)
                            throw new ArgumentException("Comment with that ID not found.");
                    }

                    command.CommandText = "SELECT TOP (1) UserId FROM Users WHERE UserId = @uId";
                    command.Parameters.AddWithValue("@uId", reference.UserId);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        if (!reader.HasRows)
                            throw new ArgumentException("User with that ID not found.");
                    }
                    //---------------------------------------------------------------<

                    reference.ReferenceId = Guid.NewGuid();
                    command.CommandText = "INSERT INTO References_t (ReferenceId, CommentId, UserId) VALUES (@referenceId, @commentId, @userId)";
                    command.Parameters.AddWithValue("@referenceId", reference.ReferenceId);
                    command.Parameters.AddWithValue("@commentId", reference.CommentId);
                    command.Parameters.AddWithValue("@userId", reference.UserId);
                    command.ExecuteNonQuery();
                    logger.Trace("log ");//logger.Trace("Logging");
                    return reference;
                }
            }
        }

        public User AddUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    //Exceptions----------------------------------------------------->
                    if (user.UserName.Length > 30)
                        throw new ArgumentException("User name too long.");

                    string wrongSymbols = "!?/.,<>%^&$*+- []{}()'`~@#\"";
                    for (int i = 0; i < wrongSymbols.Length; i++)
                        if (user.UserName.Contains(wrongSymbols[i]))
                            throw new ArgumentException("Username contains wrong symbols.");

                    command.CommandText = "SELECT TOP (1) UserId FROM Users WHERE UserName = @un";
                    command.Parameters.AddWithValue("@un", user.UserName);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        if (reader.HasRows)
                            throw new ArgumentException("That username already exists.");
                    }
                    //---------------------------------------------------------------<

                    user.UserId = Guid.NewGuid();
                    command.CommandText = "INSERT INTO Users (UserId, UserName) VALUES (@userId, @userName)";
                    command.Parameters.AddWithValue("@userId", user.UserId);
                    command.Parameters.AddWithValue("@userName", user.UserName);
                    command.ExecuteNonQuery();
                    logger.Trace("log ");//logger.Trace("Logging");
                    return user;
                }
            }
        }

        //Removal from BD--------------------------------------------------------------
        public int DeleteComment(Guid commentId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE Comments WHERE CommentId = @commentId";
                    command.Parameters.AddWithValue("@commentId", commentId);
                    return command.ExecuteNonQuery();//number of deleted items
                }
            }
        }

        public int DeleteHashtag(Guid hashtagId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE Hashtags WHERE HashtagId = @hashtagId";
                    command.Parameters.AddWithValue("@hashtagId", hashtagId);
                    logger.Trace("log ");//logger.Trace("Logging");
                    return command.ExecuteNonQuery();
                }
            }
        }

        public int DeleteLike(Guid likeId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE Likes WHERE LikeId = @likeId";
                    command.Parameters.AddWithValue("@likeId", likeId);
                    logger.Trace("log ");//logger.Trace("Logging");
                    return command.ExecuteNonQuery();
                }
            }
        }

        public int DeleteMark(Guid markId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE Marks WHERE MarkId = @markId";
                    command.Parameters.AddWithValue("@markId", markId);
                    logger.Trace("log ");//logger.Trace("Logging");
                    return command.ExecuteNonQuery();
                }
            }
        }

        public int DeletePost(Guid postId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE Posts WHERE PostId = @postId";
                    command.Parameters.AddWithValue("@postId", postId);
                    logger.Trace("log ");//logger.Trace("Logging");
                    return command.ExecuteNonQuery();
                }
            }
        }

        public int DeleteReference(Guid referenceId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE References_t WHERE ReferenceId = @referenceId";
                    command.Parameters.AddWithValue("@referenceId", referenceId);
                    logger.Trace("log ");//logger.Trace("Logging");
                    return command.ExecuteNonQuery();
                }
            }
        }

        public int DeleteUser(Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE Users WHERE UserId = @userId";
                    command.Parameters.AddWithValue("@userId", userId);
                    logger.Trace("log ");//logger.Trace("Logging");
                    return command.ExecuteNonQuery();
                }
            }
        }

        //Reading from BD--------------------------------------------------------------
        public Comment GetComment(Guid commentId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT CommentId, UserId, PostId, Date, CommentText FROM Comments WHERE CommentId = @commentId";
                    command.Parameters.AddWithValue("@commentId", commentId);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        logger.Trace("log ");//logger.Trace("Logging");
                        return new Comment
                        {
                            CommentId = reader.GetGuid(0),
                            UserId = reader.GetGuid(1),
                            PostId = reader.GetGuid(2),
                            Date = reader.GetDateTime(3),
                            CommentText = reader.GetString(4)
                        };
                    }
                }
            }
        }

        public Hashtag GetHashtag(Guid hashtagId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT HashtagId, PostId, HashtagText FROM Hashtags WHERE HashtagId = @hashtagId";
                    command.Parameters.AddWithValue("@hashtagId", hashtagId);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        logger.Trace("log ");//logger.Trace("Logging");
                        return new Hashtag
                        {
                            HashtagId = reader.GetGuid(0),
                            PostId = reader.GetGuid(1),
                            HashtagText = reader.GetString(2)
                        };
                    }
                }
            }
        }

        public Like GetLike(Guid likeId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT LikeId, UserId, PostId FROM Likes WHERE LikeId = @likeId";
                    command.Parameters.AddWithValue("@likeId", likeId);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        logger.Trace("log ");//logger.Trace("Logging");
                        return new Like
                        {
                            LikeId = reader.GetGuid(0),
                            UserId = reader.GetGuid(1),
                            PostId = reader.GetGuid(2)
                        };
                    }
                }
            }
        }

        public Mark GetMark(Guid markId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT MarkId, PostId, UserId FROM Marks WHERE MarkId = @markId";
                    command.Parameters.AddWithValue("@markId", markId);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        logger.Trace("log ");//logger.Trace("Logging");
                        return new Mark
                        {
                            MarkId = reader.GetGuid(0),
                            PostId = reader.GetGuid(1),
                            UserId = reader.GetGuid(2)
                        };
                    }
                }
            }
        }

        public Post GetPost(Guid postId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT PostId, UserId, Picture, Date FROM Posts WHERE PostId = @postId";
                    command.Parameters.AddWithValue("@postId", postId);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        logger.Trace("log ");//logger.Trace("Logging");
                        return new Post
                        {
                            PostId = reader.GetGuid(0),
                            UserId = reader.GetGuid(1),
                            Picture = (byte[]) reader["Picture"],
                            Date = reader.GetDateTime(3)
                        };
                    }
                }
            }
        }

        public Reference GetReference(Guid referenceId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT ReferenceId, CommentId, UserId FROM References_t WHERE ReferenceId = @referenceId";
                    command.Parameters.AddWithValue("@referenceId", referenceId);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        logger.Trace("log ");//logger.Trace("Logging");
                        return new Reference
                        {
                            ReferenceId = reader.GetGuid(0),
                            CommentId = reader.GetGuid(1),
                            UserId = reader.GetGuid(2)
                        };
                    }
                }
            }
        }

        public User GetUser(Guid userId)
        {
            //logger = LogManager.GetCurrentClassLogger();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT UserId, UserName FROM Users WHERE UserId = @userId";
                    command.Parameters.AddWithValue("@userId", userId);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        logger.Trace("log ");//logger.Trace("Logging");
                        return new User
                        {
                            UserId = (Guid)reader["UserId"]/*.GetGuid(0)*/,
                            UserName = (String)reader["UserName"]/*.GetString(1)*/ 
                        };

                       //logger.Trace("get user");
                    }
                }
            }
        }

        //Additional methods for comments---------------------------------------------------------->
        public Comment[] GetCommentsToPost(Guid postId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT CommentId, UserId, PostId, Date, CommentText FROM Comments WHERE PostId = @postId ORDER BY Date";
                    command.Parameters.AddWithValue("@postId", postId);
                    using (var reader = command.ExecuteReader())
                    {
                        var comments = new Comment[0];
                        int i = 0;
                        while (reader.Read())
                        {
                            Array.Resize(ref comments, comments.Length + 1);
                            comments[i] = new Comment
                            {
                                CommentId = reader.GetGuid(0),
                                UserId = reader.GetGuid(1),
                                PostId = reader.GetGuid(2),
                                Date = reader.GetDateTime(3),
                                CommentText = reader.GetString(4)
                            };
                            i++;
                        }
                        logger.Trace("log ");//logger.Trace("Logging");
                        return comments;
                    }
                }
            }
        }

        public Comment[] GetCommentsOfUser(Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT CommentId, UserId, PostId, Date, CommentText FROM Comments WHERE UserId = @userId ";
                    command.Parameters.AddWithValue("@userId", userId);
                    using (var reader = command.ExecuteReader())
                    {
                        var comments = new Comment[0];
                        int i = 0;
                        while (reader.Read())
                        {
                            Array.Resize(ref comments, comments.Length + 1);
                            comments[i] = new Comment
                            {
                                CommentId = reader.GetGuid(0),
                                UserId = reader.GetGuid(1),
                                PostId = reader.GetGuid(2),
                                Date = reader.GetDateTime(3),
                                CommentText = reader.GetString(4)
                            };
                            i++;
                        }
                        logger.Trace("log ");//logger.Trace("Logging");
                        return comments;
                    }
                }
            }
        }
        //-----------------------------------------------------------------------<

        //Additional methods for posts-------------------------------------------->
        public Post[] GetAllPosts()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT PostId, UserId, Picture, Date FROM Posts ORDER BY Date DESC"; /*PostId, UserId, Picture, Date*/
                    using (var reader = command.ExecuteReader())
                    {
                        var posts = new Post[0];
                        int i = 0;
                        while (reader.Read())
                        {
                            Array.Resize(ref posts, posts.Length + 1);
                            posts[i] = new Post
                            {
                                PostId = reader.GetGuid(0),
                                UserId = reader.GetGuid(1),
                                Picture = (byte[])reader["Picture"],
                                Date = reader.GetDateTime(3),
                            };
                            i++;
                        }
                        logger.Trace("log ");//logger.Trace("Logging");
                        return posts;
                    }
                }
            }
        }

        public Post[] GetPostsOfUser(Guid userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT PostId, UserId, Picture, Date  FROM Posts WHERE UserId = @userId ORDER BY Date DESC"; /*PostId, UserId, Picture, Date*/
                    command.Parameters.AddWithValue("@userId", userId);
                    using (var reader = command.ExecuteReader())
                    {
                        var posts = new Post[0];
                        int i = 0;
                        while (reader.Read())
                        {
                            Array.Resize(ref posts, posts.Length + 1);
                            posts[i] = new Post
                            {
                                PostId = reader.GetGuid(0),
                                UserId = reader.GetGuid(1),
                                Picture = (byte[])reader["Picture"],
                                Date = reader.GetDateTime(3),
                            };
                            i++;
                        }
                        logger.Trace("log ");//logger.Trace("Logging");
                        return posts;
                    }
                }
            }
        }
        //-----------------------------------------------------------------------<

        //additional methods for users------------------------------------------->
        public User GetUserByName(string userName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT UserId, UserName FROM Users WHERE UserName = @userName";
                    command.Parameters.AddWithValue("@userName", userName);
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        logger.Trace("log ");//logger.Trace("Logging");
                        if (reader.HasRows)
                            return new User
                            {
                                UserId = (Guid)reader["UserId"]/*.GetGuid(0)*/,
                                UserName = (String)reader["UserName"]/*.GetString(1)*/
                            };
                        else
                            return new User { };
                    }
                }
            }
        }
        //-----------------------------------------------------------------------<

    }
}
