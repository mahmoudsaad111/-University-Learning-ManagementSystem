﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.RealTimeInterfaces.CommentReplyInLecture
{
    public class AddCommentReplyInLectureSenderMessage
    {
        public string SenderUserName { get; set; }
        public int LectureId { get; set; }
        public string CommentReplyContent { get; set;  }
        public int CommentId { get; set; }
    }
}
