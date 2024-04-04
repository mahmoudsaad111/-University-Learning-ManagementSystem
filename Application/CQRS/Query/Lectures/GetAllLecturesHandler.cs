﻿using Application.Common.Interfaces.CQRSInterfaces;
using Application.Common.Interfaces.Presistance;
using Application.CQRS.Query.Lectures;
using Domain.Models;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Query.Lectures
{
    public class GetAllLecturesHandler : IQueryHandler<GetAllLecturesQuery , IEnumerable<Lecture>>
    {
        private readonly IUnitOfwork unitOfwork;

        public GetAllLecturesHandler(IUnitOfwork unitOfwork)
        {
            this.unitOfwork = unitOfwork;
        }

        public async Task<Result<IEnumerable<Lecture>>> Handle(GetAllLecturesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var Lectures = await unitOfwork.LectureRepository.FindAllAsyncInclude();
                return Result.Create<IEnumerable<Lecture>>(Lectures);
            }
            catch (Exception ex)
            {
                return Result.Failure<IEnumerable<Lecture>>(new Error(code: "GetAllLectures", message: ex.Message.ToString()));
            }
        }
    }
}
