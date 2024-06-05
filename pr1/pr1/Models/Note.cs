using System;
using System.Collections.Generic;
using System.Text;

using SQLite;
namespace pr1.Models
{
    public class Note
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int Importance { get; set; }
    
        public string Category { get; set; }

    
    }
}
