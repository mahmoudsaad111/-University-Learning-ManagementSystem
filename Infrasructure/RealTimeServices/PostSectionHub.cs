﻿using Application.Common.Interfaces.Presistance;
using Application.Common.Interfaces.RealTimeInterfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using System.Runtime.InteropServices;
using Application.Common.Interfaces.RealTimeInterfaces.PostInSection;
using Domain.Models;
using static System.Collections.Specialized.BitVector32;


namespace Infrastructure.RealTimeServices
{
    public class PostSectionHub : Hub, IPostSectionHub
    {
        private readonly IUnitOfwork unitOfwork;
        private readonly ICheckDataOfRealTimeRequests checkDataOfRealTimeRequests;

        public PostSectionHub(IUnitOfwork unitOfwork, ICheckDataOfRealTimeRequests checkDataOfRealTimeRequests)
        {
            this.unitOfwork = unitOfwork;
            this.checkDataOfRealTimeRequests = checkDataOfRealTimeRequests;
        }

        public async override Task OnConnectedAsync()
        {
            if (Context is not null)
            {
                string connectionId = Context.ConnectionId;
                var userName = Context?.User?.Identity?.Name;
                var sectionIdString = Context?.GetHttpContext()?.Request.Query["SectionId"];

                if (userName is not null && !string.IsNullOrEmpty(sectionIdString))
                {
                    var TypeOfuserAndId = await checkDataOfRealTimeRequests.GetTypeOfUserAndHisId(userName);

                    if (TypeOfuserAndId is not null & int.TryParse(sectionIdString, out int sectionId))
                    {
                        bool IfUserInThisSection = await checkDataOfRealTimeRequests.CheckIfUserInSection(SectionId: sectionId,
                            UserId: TypeOfuserAndId.Item2.Id, typesOfUsers: TypeOfuserAndId.Item1);

                        // Add the user to the corresponding section group
                        if (IfUserInThisSection)
                        {
                            await Groups.AddToGroupAsync(connectionId, $"Section-{sectionId}");
                        }
                    }
                }
            }
            await base.OnConnectedAsync();
        }

        public async void AddPostInSection(PostAddSenderMessage postMessage)
        {
            var TypeOfuserAndId = await checkDataOfRealTimeRequests.GetTypeOfUserAndHisId(postMessage.SenderUserName);

            if (TypeOfuserAndId is not null && (TypeOfuserAndId.Item1 == TypesOfUsers.Professor
                || TypeOfuserAndId.Item1 == TypesOfUsers.Instructor))
            {

                User user = TypeOfuserAndId.Item2;

                var post = new Post
                {
                    Content = postMessage.PostContent,
                    CreatedBy = postMessage.SenderUserName,
                    IsProfessor = true ? TypeOfuserAndId.Item1 == TypesOfUsers.Professor : false,
                    SectionId = postMessage.SectionId,
                    PublisherId = user.Id

                };
                try
                {
                    var postInDB = await unitOfwork.PostRepository.CreateAsync(post);
                    await unitOfwork.SaveChangesAsync();
                    var postMessageforClinets = new PostAddReceiverMessage
                    {
                        SenderImage = user.ImageUrl,
                        SenderName = user.FullName,
                        SenderUserName = user.UserName,
                        PostContent = post.Content,
                        PostId = postInDB.PostId

                    };
                    await Clients.Group($"Section-{postMessage.SectionId}").SendAsync("AddSectionPost", postMessageforClinets);
                }
                catch (Exception ex)
                {

                }
            }

        }

        public async void DeletePostInSection(PostDeleteSenderMessage postMessage)
        {
            var TypeOfuserAndId = await checkDataOfRealTimeRequests.GetTypeOfUserAndHisId(postMessage.SenderUserName);

            var post = await unitOfwork.PostRepository.GetByIdAsync(postMessage.PostId);

            // If there is post with the given id and the publisher is the same who want to delete it then OK 

            if (post != null && post.PublisherId == TypeOfuserAndId.Item2.Id)
            {
                try
                {
                    bool IsDeleted = await unitOfwork.PostRepository.DeleteAsync(postMessage.PostId);
                    if (IsDeleted)
                    {
                        await unitOfwork.SaveChangesAsync();
                        PostDeleteReceiverMessage postReceiverMessage = new PostDeleteReceiverMessage
                        {
                            PostId = postMessage.PostId,
                        };

                        await Clients.Group($"Section-{postMessage.SectionId}").SendAsync("DeleteSectionPost", postReceiverMessage);
                    }
                }
                catch (Exception ex)
                {

                }
            }

        }

        public async void UpdatePostInSection(PostUpdateSenderMessage postMessage)
        {
            var TypeOfuserAndId = await checkDataOfRealTimeRequests.GetTypeOfUserAndHisId(postMessage.SenderUserName);

            var post = await unitOfwork.PostRepository.GetByIdAsync(postMessage.PostId);

            // If there is post with the given id and the publisher is the same who want to delete it then OK 

            if (post != null && post.PublisherId == TypeOfuserAndId.Item2.Id)
            {
                post.Content = postMessage.PostContent;

                try
                {
                    bool IsUpdated = await unitOfwork.PostRepository.UpdateAsync(post);

                    if (IsUpdated)
                    {
                        await unitOfwork.SaveChangesAsync();
                        PostUpdateReceiverMessage postReceiverMessage = new PostUpdateReceiverMessage
                        {
                            PostId = postMessage.PostId,
                            PostContent = postMessage.PostContent,
                        };

                        await Clients.Group($"Section-{postMessage.SectionId}").SendAsync("UpdateSectionPost", postReceiverMessage);
                    }
                }
                catch (Exception ex)
                {

                }
            }

        }
    }
}

// Last work ; 
/*
    public override async Task OnConnectedAsync()
        {
            string ConnectionId = Context.ConnectionId;
            var userName = Context?.User?.Identity?.Name;
            var role = "df";
            //var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userName is not null)
            {
                var user =await unitOfwork.UserRepository.GetUserByUserName(userName);         
                if (user is not null)
                {
                    IEnumerable<int> SectionsId = null; 
                    if(user.Student is not null)
                    {
                        int StudentId = user.Student.StudentId;  
                        SectionsId = await unitOfwork.StudentSectionRepository.GetAllSectionsIdofStudent(StudentId);          
                    }

                    else if(user.Instructor is not null)
                    {
                        int InstrctorId = user.Instructor.InstructorId;
                        SectionsId=await unitOfwork.SectionRepository.GetAllSectionsIdOfInstructore(InstrctorId);
                    }
                    else if(user.Professor is not null)
                    {
                        int ProfessorId = user.Professor.ProfessorId;
                        SectionsId =await unitOfwork.SectionRepository.GetAllSectionsIdOfProfessor(ProfessorId);
                    }

                    foreach (var sectionId in SectionsId)
                    {
                        await Groups.AddToGroupAsync(ConnectionId, $"Section{sectionId}");
                    }
                }
            }

            await base.OnConnectedAsync();
        }
 */