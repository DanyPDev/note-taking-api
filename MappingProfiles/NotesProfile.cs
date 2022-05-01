using AutoMapper;
using NoteTakingApp.Dtos;
using NoteTakingApp.Models;

namespace NoteTakingApp.MappingProfiles
{
    public class NotesProfile : Profile
    {
        public NotesProfile()
        {   

            //We want to transform from this object to that object (Source ---> Target)
            CreateMap<Note, NoteReadDto>();
            CreateMap<NoteCreateDto, Note>();
            CreateMap<Note, NoteCreateDto>();
        }
    }
}