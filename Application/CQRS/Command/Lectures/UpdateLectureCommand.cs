﻿using Application.Common.Interfaces.CQRSInterfaces;
using Contract.Dto.Lectures;
using Domain.Models;


namespace Application.CQRS.Command.Lectures
{
    public class UpdateLectureCommand  : ICommand<Lecture>
	{
		public int Id { get; set; }
        public string CreatorUserName { get; set; }
        public LectureDto LectureDto { get; set; }
	}
}
