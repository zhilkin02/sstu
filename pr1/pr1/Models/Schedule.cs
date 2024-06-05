using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using pr1.Views;
using System.Collections.ObjectModel;
namespace pr1.Models
{
    public class Schedule
    {
  
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Hour { get; set; }
        public string Subject { get; set; }
        public string Type { get; set; }
        public string Room { get; set; }
        public string Teacher { get; set; }
        public string Assignment { get; set; }

        public string DayName { get; set; } // День недели  
        public int WeekNumber { get; set; }   // Номер недели
        public string Date { get; set; }    // Дата
    }
}

