using pr1.Models;
using pr1.Views;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace pr1.Data
{
    public class ScheduleDB
    {
        private SQLiteConnection database;
        private static object locker = new object();
        private readonly SQLiteConnection conn;


        public ScheduleDB(string connectionString)
        {
           
            conn = new SQLiteConnection(connectionString);
            conn.CreateTable<Schedule>();

        }
        //public ScheduleDB()
        //{
        //    // Путь к базе данных SQLite
        //    string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ScheduleDB.db3");
        //    // Инициализация подключения к базе данных
        //    database = new SQLiteConnection(databasePath);
        //    // Создание таблицы Schedule, если она не существует
        //    database.CreateTable<Schedule>();
        //}

        // Метод для добавления расписания в базу данных
        //public void AddSchedule(List<Schedule> scheduleItems)
        //{
        //    lock (locker)
        //    {
        //        // Добавление расписания в базу данных
        //        foreach (var scheduleItem in scheduleItems)
        //        {
        //            database.Insert(scheduleItem);
        //        }
        //    }
        //}
        public List<Schedule> GetScheduls()
        {
            return conn.Table<Schedule>().ToList();
        }

        public int SaveSchedule(Schedule scheduls)
        {
            if (scheduls.Id != 0)
            {
                return conn.Update(scheduls);
            }
            else
            {
                return conn.Insert(scheduls);
            }
        }
        public void SaveSchedules(List<Schedule> schedules)
        {

            conn.InsertAll(schedules);
        }
        public Schedule GetSchedule(int id)
        {
            return conn.Table<Schedule>()
                .Where(i => i.Id == id)
                .FirstOrDefault();
        }
        // Метод для удаления старых записей расписания
        public void DeleteAllSchedules()
        {
            conn.DeleteAll<Schedule>();
        }

        public void RemoveOldSchedule()
        {
            // Определяем текущую дату
            DateTime currentDate = DateTime.Now;
            // Вычисляем дату, которая находится за две недели от текущей даты
            DateTime twoWeeksAgo = currentDate.AddDays(-14);

            // Получаем все записи расписания из базы данных
            var scheduleItems = conn.Table<Schedule>().ToList();

            // Проходим по каждой записи расписания
            foreach (var scheduleItem in scheduleItems)
            {
                // Преобразуем строку с датой к формату "день.месяц"
                string fullDateString = $"{scheduleItem.Date}.{currentDate.Year}";
                // Парсим строку с датой и добавляем к ней фиксированный год
                if (DateTime.TryParseExact(fullDateString, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime scheduleDate))
                {
                    // Проверяем, дата расписания меньше чем две недели назад
                    if (scheduleDate < twoWeeksAgo)
                    {
                        // Удаляем запись из базы данных
                        conn.Delete(scheduleItem);
                    }
                }
            }
        }

        public List<Schedule> GetSchedulesBySubject(string subject, string type,string assignment)
        {
            lock (locker)
            {
                var schedules = conn.Table<Schedule>().Where(x => (x.Subject == subject && x.Type == type)).ToList();
                foreach (var schedule in schedules)
                {
                    if (assignment != null)
                    {
                        Console.WriteLine("note: " + assignment);

                        schedule.Assignment = assignment;
                    }
                    else
                        schedule.Assignment = " ";
                    conn.Update(schedule); // Сохраняем изменения в базе данных
                }
                return schedules;
            }
        }

    }
}
