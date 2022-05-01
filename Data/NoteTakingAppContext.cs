using Microsoft.EntityFrameworkCore;
using NoteTakingApp.Models;

namespace NoteTakingApp.Data
{
    public class NoteTakingAppContext : DbContext
    {
        public NoteTakingAppContext(DbContextOptions<NoteTakingAppContext> options) : base(options)
        {
         
        }

        //representation of Note.cs model to DB as a DbSet
        public DbSet<Note>? Notes { get; set; }
    }
}