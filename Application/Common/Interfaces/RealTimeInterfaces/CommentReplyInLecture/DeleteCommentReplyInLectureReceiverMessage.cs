﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.RealTimeInterfaces.CommentReplyInLecture
{
    public class DeleteCommentReplyInLectureReceiverMessage
    {
        public int CommentId { get; set; }
        public int CommentReplyId { get; set; }  
    }
}
