using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstLikeApp.Model
{
    public class Reference
    {
        public Guid ReferenceId { get; set; }
        public Guid CommentId { get; set; }
        public Guid UserId { get; set; }
    }
}
