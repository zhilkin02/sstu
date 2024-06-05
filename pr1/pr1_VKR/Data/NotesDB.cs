using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using pr1.Models;
using System.Threading.Tasks;

namespace pr1.Data
{
   public class NotesDB
    {
        private readonly SQLiteConnection conn;

        // readonly SQLiteAsyncConnection db;
        public NotesDB(string connectionString) 
        {
        //    db = new SQLiteAsyncConnection(connectionString);
         //   db.CreateTableAsync<Note>().Wait();
         conn= new SQLiteConnection(connectionString);
            conn.CreateTable<Note>();
        
        }

        //public Task<List<Note>> GetNotesAsync() { 
        //    return db.Table<Note>().ToListAsync();
        //}

        //public Task<Note> GetNoteAsync(int id)
        //{
        //    return db.Table<Note>()
        //        .Where(i => i.Id == id)
        //        .FirstOrDefaultAsync();
        //}
        //public Task<int> SaveNoteAsync(Note note)
        //{
        //    if(note.Id != 0)
        //    {
        //        return db.UpdateAsync(note);
        //    }
        //    else
        //    {
        //        return db.InsertAsync(note);
        //    }
        //}

        //public Task<int> DeleteNoteAsync(Note note)
        //{
        //    return db.DeleteAsync(note);
        //}

        public List<Note> GetNotes()
        {
            return conn.Table<Note>().ToList();
        }
        public int SaveNote(Note note)
        {
            if (note.Id != 0)
            {
                return conn.Update(note);
            }
            else
            {
                return conn.Insert(note);
            }
        }

       

        public Note GetNote(int id)
        {
            return conn.Table<Note>()
                .Where(i => i.Id == id)
                .FirstOrDefault();
        }

        public int DeleteNote(Note note)
        {
            return conn.Delete(note);
        }
    }
}
