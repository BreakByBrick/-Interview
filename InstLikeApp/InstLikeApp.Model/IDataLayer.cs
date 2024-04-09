using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstLikeApp.Model
{
    public interface IDataLayer
    {
        User AddUser(User user);
        Post AddPost(Post post);
        Comment AddComment(Comment comment);
        Hashtag AddHashtag(Hashtag hashtag);
        Like AddLike(Like like);
        Mark AddMark(Mark mark);
        Reference AddReference(Reference reference);

        User GetUser(Guid userId);
        Post GetPost(Guid postId);
        Comment GetComment(Guid commentId);
        Hashtag GetHashtag(Guid hashtagId);
        Like GetLike(Guid likeId);
        Mark GetMark(Guid markId);
        Reference GetReference(Guid referenceId);

        int DeleteUser(Guid userId);
        int DeletePost(Guid postId);
        int DeleteComment(Guid commentId);
        int DeleteHashtag(Guid hashtagId);
        int DeleteLike(Guid likeId);
        int DeleteMark(Guid markId);
        int DeleteReference(Guid referenceId);

        Comment[] GetCommentsToPost(Guid postId);
        Comment[] GetCommentsOfUser(Guid userId);

        Post[] GetAllPosts();
        Post[] GetPostsOfUser(Guid userId);

        User GetUserByName(string userName);
    }
}
