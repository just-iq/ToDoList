using AutoMapper;
using ToDoList.Dto;
using ToDoList.Models;

namespace ToDoList.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<TaskItem, TaskItemDto>();
            CreateMap<TaskItemDto, TaskItem>();
        }
    }
}
