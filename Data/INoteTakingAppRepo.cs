using NoteTakingApp.Models;

namespace NoteTakingApp.Data
{

    public interface INoteTakingAppRepo
    {
        IEnumerable<Note> GetAllNotes();

        Note GetNoteById(int id);    

        void CreateNote(Note note);

        void UpdateNote(Note note);

        void DeleteNote(Note note);    

        bool SaveChanges();

        
    }
}