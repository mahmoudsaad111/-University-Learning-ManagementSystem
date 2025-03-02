﻿using Application.Common.Interfaces.CQRSInterfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Query.CourseCycles
{
    public class GetCourseCycleIdFromCourseIdAndGroupIdQuery :IQuery<CourseCycle>
    {
        public int CourseId { get; set; }   
        public int GroupId { get; set; }
    }
}
