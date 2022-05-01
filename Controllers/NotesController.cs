using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using NoteTakingApp.Data;
using NoteTakingApp.Dtos;
using NoteTakingApp.Models;


namespace NoteTakingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly INoteTakingAppRepo _repository;
        
        //Automapper Dependency injected into Controller class in order to easily transform queried data from database into a Data Trannsfert Object, which depending on the contract, may change
        private readonly IMapper _mapper; //See .MappingProfiles/NoteProfile to see how objects are mapped, we just need to pass the desired return type in Generic parameter for .Map<ObjectType>() and pass the object to be transformed in the .Map<T>() method  



        public NotesController(INoteTakingAppRepo repository, IMapper mapper) //we setup MockNoteTakingAppRepo to be injected via builder.Services.AddScoped<INoteTakingAppRepo, MockNoteTakingAppRepo>() in Program.cs
        {
              _repository = repository;
              _mapper = mapper;
        }

        //public readonly MockNoteTakingAppRepo _repository = new MockNoteTakingAppRepo();
        
        
        [HttpGet]
        public ActionResult <IEnumerable<NoteReadDto>> GetAllNotes()
        {
            var noteItems = _repository.GetAllNotes();
            return Ok(_mapper.Map<IEnumerable<NoteReadDto>>(noteItems));
        }


        [HttpGet("{id}", Name="GetNoteById")]
        public ActionResult <NoteReadDto> GetNoteById(int id)
        {
            var noteItem = _repository.GetNoteById(id);
            if(noteItem != null)
                return Ok(_mapper.Map<NoteReadDto>(noteItem));
        
            return NotFound();
        }
    
        [HttpPost]
        public ActionResult <NoteCreateDto> CreateNote(NoteCreateDto noteCreateDto)
        {
            var noteModel = _mapper.Map<Note>(noteCreateDto);
            _repository.CreateNote(noteModel);
            _repository.SaveChanges();
            var NoteReadDto = _mapper.Map<NoteReadDto>(noteModel);

            //generates 201 Created response but also append uri pointing to the newly created ressource
            return CreatedAtRoute(nameof(GetNoteById), new {Id = NoteReadDto.Id}, NoteReadDto); //basically first parameter points to the newly created data object 
        }

        [HttpPut("{id}")]
        public ActionResult UpdateNote(int id, NoteCreateDto noteCreateDto)
        {
            var noteModelFromRepo = _repository.GetNoteById(id);

            //does ressource with id value exist?
            if(noteModelFromRepo == null)
                return NotFound();

            //copying noteCreateDto fields into model from repo using automapping (noteCreateDto.field ----> noteModelFromRepo.field )
            _mapper.Map(noteCreateDto, noteModelFromRepo);

            //some imlementations may require we do this
            //_repository.UpdateNote(noteModelFromRepo);

            //actually flush data to DB
            _repository.SaveChanges();

            return NoContent();

        }


        [HttpPatch("{id}")]
        public ActionResult PartialNoteUpdate(int id, JsonPatchDocument<NoteCreateDto> patchDoc) //we receive patchdoc from client
        {
            var noteModelFromRepo = _repository.GetNoteById(id);

            //does ressource with id value exist?
            if(noteModelFromRepo == null)
                return NotFound();

            
            //noteToPatch is a NoteCreateDto instance(we only dont want id attribute)
            var noteToPatch = _mapper.Map<NoteCreateDto>(noteModelFromRepo);
            patchDoc.ApplyTo(noteToPatch, ModelState); // ModelState ensure that model is valid

            //validate model
            if(!TryValidateModel(noteToPatch))
                return ValidationProblem(ModelState);
            
            //copy note to Patch attributes into noteModelFromRepo, effectively updating it
            _mapper.Map(noteToPatch, noteModelFromRepo);
            
            //actually flush data to DB
            _repository.SaveChanges();


            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteNote(int id)
        {
            //find ressource with this id
            var noteModelFromRepo = _repository.GetNoteById(id);

            //does ressource with id value exist?
            if(noteModelFromRepo == null)
                return NotFound();

            //delete using repository class DeleteNodeMethod
            _repository.DeleteNote(noteModelFromRepo);
            _repository.SaveChanges(); //actually flush data to DB

            return NoContent();

        }
    }
}