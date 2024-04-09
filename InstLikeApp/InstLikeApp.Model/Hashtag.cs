using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstLikeApp.Model
{
    public class Hashtag
    {
        public Guid HashtagId { get; set; }
        public Guid PostId { get; set; }
        public string HashtagText { get; set; }
    }
}
