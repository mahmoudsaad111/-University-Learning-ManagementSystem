﻿using Application.Common.Interfaces.CQRSInterfaces;
using Contract.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Query.Professors
{
    public class GetNameIdProfessorsQuery : IQuery<IEnumerable<NameIdDto>>
    {
        public int DepartementId { get; set; }
    }
}
