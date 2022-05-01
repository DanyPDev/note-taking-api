using NoteTakingApp.Models;

namespace NoteTakingApp.Data
{
    public class PostgreSQLNoteTakingAppRepo : INoteTakingAppRepo
    {
        private readonly NoteTakingAppContext _context;

        public PostgreSQLNoteTakingAppRepo(NoteTakingAppContext context)
        {
            this._context = context;
        }

        public void CreateNote(Note note)
        {
            if(note == null)
                throw new ArgumentNullException(nameof(note));

            _context.Notes.Add(note);          
        }

        public void DeleteNote(Note note)
        {
            if(note == null)
                throw new ArgumentNullException(nameof(note));
                
            _context.Notes.Remove(note);    
        }

        public IEnumerable<Note> GetAllNotes()
        {
                return _context.Notes.ToList();
        }

        public Note GetNoteById(int id)
        {
            return _context.Notes.FirstOrDefault(p => p.Id == id);
        }


        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }


        public void UpdateNote(Note note)
        {
        
        }
    }
}