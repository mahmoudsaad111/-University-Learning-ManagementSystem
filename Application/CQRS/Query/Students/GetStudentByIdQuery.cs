﻿using Application.Common.Interfaces.CQRSInterfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Query.Students
{
	public class GetStudentByIdQuery :IQuery<Student>
	{
		public int Id { get; set; }	
	}
}
