using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstLikeApp.Model
{
    public class Like
    {
        public Guid LikeId { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
    }
}
