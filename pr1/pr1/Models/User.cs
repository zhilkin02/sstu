using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace pr1.Models
{
    public class User
    {
        [PrimaryKey] //  атрибут для указания первичного ключа
        public int Id { get; set; } // Первичного ключ
        //public string Name { get; set; }
        public string Group { get; set; }
        public string Teacher { get; set; }

        public string URL { get; set; }
        public string TeacherURL { get; set; }
        public string Exams { get; set; }
        public int Sch {  get; set; }   

    }
}
