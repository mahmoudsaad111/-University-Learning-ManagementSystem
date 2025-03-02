﻿using Application.Common.Interfaces.CQRSInterfaces;
using Application.enums;
using Domain.TmpFilesProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Query.FileResources
{
    public class GetAllFilesOfEntityHasFilesQuery : IQuery<IEnumerable<IFileResourceModel>>
    {
        public EntitiesHasFiles TypeOfEntity { get; set; }
        public int LectureId { get; set; }  
        public int AssignmentId { get; set; }   
        public int AssignmentAnswerId {  get; set; }    
    }
}
