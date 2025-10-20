using AsmPj.Models;
using AsmPj.Models.Dtos;
using AutoMapper;

namespace AsmPj.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Board
        CreateMap<Board, CreateBoardRequest>();
        CreateMap<Board, UpdateBoardRequest>();
        CreateMap<CreateBoardRequest, Board>();
        CreateMap<UpdateBoardRequest, Board>();
        CreateMap<Board, BoardResponse>();
        CreateMap<BoardResponse, Board>();
        
        // TaskItem
        CreateMap<TaskItem, TaskRequest>();
        // CreateMap<TaskItem, UpdateTaskRequest>();
        CreateMap<TaskRequest, TaskItem>();
        // CreateMap<UpdateTaskRequest, TaskItem>();
        CreateMap<TaskItem, TaskResponse>();
        CreateMap<TaskResponse, TaskItem>();
    }
}