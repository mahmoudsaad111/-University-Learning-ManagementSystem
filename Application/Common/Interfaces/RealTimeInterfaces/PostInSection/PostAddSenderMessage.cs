﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.RealTimeInterfaces.PostInSection
{
    public class PostAddSenderMessage
    {
        public int SectionId { get; set; } 
        public string SenderUserName { get; set; }

        public string PostContent { get; set; }

    }
}