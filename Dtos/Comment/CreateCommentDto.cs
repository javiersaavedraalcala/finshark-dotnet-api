using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finshark.Dtos.Comment
{
    public class CreateCommentDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}