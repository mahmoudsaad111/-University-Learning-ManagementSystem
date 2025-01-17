﻿using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Dto.MCQs
{
    public class MCQInfoWithStudentAnswerDto
    {
        public string Text { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public int Degree { get; set;  }
        public MCQOptions CorrectAnswer { get; set; }
        public MCQOptions StudentAnswer { get; set; }   
    }
}
