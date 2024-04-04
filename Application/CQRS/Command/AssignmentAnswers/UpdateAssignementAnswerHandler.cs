﻿using Application.Common.Interfaces.CQRSInterfaces;
using Application.Common.Interfaces.Presistance;
using Domain.Models;
using Domain.Shared;
 

namespace Application.CQRS.Command.AssignementAnswers
{
	public class UpdateDepartementHandler : ICommandHandler<UpdateAssignementAnswerCommand, AssignmentAnswer>
	{
		private readonly IUnitOfwork unitOfwork; 
		public UpdateDepartementHandler (IUnitOfwork unitOfwork)
		{
			this.unitOfwork = unitOfwork;
		}
		public async Task<Result<AssignmentAnswer>> Handle(UpdateAssignementAnswerCommand request, CancellationToken cancellationToken)
		{
			try
			{

				AssignmentAnswer assignementanswer = await unitOfwork.AssignementAnswerRepository.FindAsync(f => f.AssignmentAnswer_id == request.Id);
				if (assignementanswer is null)
					return Result.Failure<AssignmentAnswer>(new Error(code: "Update AssignementAnswer", message: "No AssignementAnswer exist by this Id"));

				if(assignementanswer.StudentId != request.AssignementAnswerDto.StudentId || assignementanswer.AssignmentId!=request.AssignementAnswerDto.AssignementId)
                    return Result.Failure<AssignmentAnswer>(new Error(code: "Update AssignementAnswer", message: "Can not change Student or assignement"));

				assignementanswer.Description = request.AssignementAnswerDto.Description;

				int NumOfTasks  = await unitOfwork.SaveChangesAsync();
				return Result.Success<AssignmentAnswer>(assignementanswer);
			}
			catch (Exception ex)
			{
				return Result.Failure<AssignmentAnswer>(new Error(code: "Updated AssignementAnswer" , message: ex.Message.ToString())); 
			}
		}
	}
}