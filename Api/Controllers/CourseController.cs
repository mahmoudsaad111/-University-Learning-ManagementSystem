﻿using Application.CQRS.Command.Courses;
using Application.CQRS.Query.Courses;
using Application.CQRS.Query.Professors;
using Application.CQRS.Query.Students;
using Contract.Dto.Courses;
using Domain.Models;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]

    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {

        private readonly IMediator mediator;
        private readonly UserManager<User> userManager;

        public CourseController(IMediator mediator, UserManager<User> userManager)
        {
            this.mediator = mediator;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("CreateCourse")] //CreateCourseCommand
        public async Task<ActionResult> CreateCourse([FromBody] CourseDto courseDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var command = new CreateCourseCommand { CourseDto = courseDto };
                Result result = await mediator.Send(command);

                return result.IsSuccess ? Ok("Course Added Sucessfully") : BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("GetCourses")]
        public async Task<ActionResult> GetALlCourses()
        {
            try
            {
                var result = await mediator.Send(new GetAllCoursesQuery());
                if (result.IsSuccess)
                    return Ok(result.Value);
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet]
        [Route("GetCoursesOfAcadimicYear")]
        public async Task<ActionResult> GetALlCoursesOfAcadimicYear([FromHeader] int AcadimicYearId , [FromQuery] int CourseCategoryId)
        {
            try
            {
                var result = await mediator.Send(new GetAllCoursesOfAcadimicYearQuery { AcadimicYearId=AcadimicYearId,CourseCategoryId=CourseCategoryId});
                if (result.IsSuccess)
                    return Ok(result.Value);
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet]
        [Route("GetAllCoursesOfStudent")]
        public async Task<ActionResult> GetAllCoursesOfStudent([FromHeader] string StudentUserName)
        {
            try
            {
                var user= await userManager.FindByNameAsync(StudentUserName);
                if (user is null )
                    return BadRequest("Wrong userName");

                var ResultOfGetStudent = await mediator.Send(new GetStudentByIdQuery { Id=user.Id });

                if (ResultOfGetStudent is null || ResultOfGetStudent.IsFailure || ResultOfGetStudent.Value is null)
                    return BadRequest("Wrong userName2");

                var Student= ResultOfGetStudent.Value;

                var result = await mediator.Send(new GetAllCoursesOfStudentQuery {StudentId = Student.StudentId  });
                if (result.IsSuccess)
                    return Ok(result.Value);
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }



        [HttpGet]
        [Route("GetAllCoursesOfProfessor")]
        public async Task<ActionResult> GetAllCoursesOfProfessor([FromHeader] string ProfessorUserName)
        {
            try
            {
                var user = await userManager.FindByNameAsync(ProfessorUserName);
                if (user is null)
                    return BadRequest("Wrong userName");

                var ResultOfGetProfessor = await mediator.Send(new GetProfessorByIdQuery { Id = user.Id });

                if (ResultOfGetProfessor is null || ResultOfGetProfessor.IsFailure || ResultOfGetProfessor.Value is null)
                    return BadRequest("Wrong userName2");

                var Professor = ResultOfGetProfessor.Value;

                var result = await mediator.Send(new GetAllCoursesOfProfessorQuery { ProfessorId = Professor.ProfessorId }) ;
                if (result.IsSuccess)
                    return Ok(result.Value);
                return BadRequest(result.Error);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }



        [HttpPut]
        [Route("UpdateCourse")]
        public async Task<ActionResult> UpdateCourse([FromHeader] int Id, [FromBody] CourseDto courseDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                Result<Course> resultOfUpdated = await mediator.Send(new UpdateCourseCommand { Id = Id, courseDto = courseDto });

                return resultOfUpdated.IsSuccess ? Ok(resultOfUpdated.Value) : BadRequest(resultOfUpdated.Error);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("DeleteCourse")]
        public async Task<ActionResult> DeleteCourse([FromHeader] int Id )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (Id == 0)
                return BadRequest("Enter valid ID");
            try
            {
                Result<int> resultOfDeleted = await mediator.Send(new DeleteCourseCommand { Id = Id });
                return resultOfDeleted.IsSuccess ? Ok(resultOfDeleted.Value) : BadRequest("un valid data");
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
