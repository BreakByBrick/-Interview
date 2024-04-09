using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstLikeApp.Model
{
    public class Post
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public byte[] Picture { get; set; }
        public DateTime Date { get; set; }
    }
}
