using pr1.Models;
using SQLite;

namespace pr1.Data
{
    public class UserDB
    {
        private readonly SQLiteConnection conn;

        public UserDB(string connectionString)
        {
            conn = new SQLiteConnection(connectionString);
            conn.CreateTable<User>() ;

        }

        public User GetUser()
        {
            return conn.Table<User>()
                .FirstOrDefault();
        }

        //public void AddOrUpdateName(string name)
        //{
        //    if (name == null || name.Trim() == "") name = "Пользователь";
        //    var user = GetUser();
        //    if (user == null)
        //    {
        //        user = new User { Name = name.Trim() };
        //        conn.Insert(user);
        //    }
        //    else
        //    {
        //        user.Name = name;
        //        conn.Update(user);
        //    }
        //}

        public void AddOrUpdateGroup(string group)
        {
            var user = GetUser();
            if (user == null)
            {
                user = new User { Group = group };
                conn.Insert(user);
            }
            else
            {
                user.Group = group;
                conn.Update(user);
            }
        }
        public void AddOrUpdateSch(int item)
        {
            var user = GetUser();
            if (user == null)
            {
                user = new User { Sch = item };
                conn.Insert(user);
            }
            else
            {
                user.Sch = item;
                conn.Update(user);
            }
        }
        public void AddOrUpdateExams(string exams)
        {
            var user = GetUser();
            if (user == null)
            {
                user = new User { Exams = exams };
                conn.Insert(user);
            }
            else
            {
                user.Exams = exams;
                conn.Update(user);
            }
        }
        public void AddOrUpdateTeacher(string teacher)
        {
            var user = GetUser();
            if (user == null)
            {
                user = new User { Teacher = teacher };
                conn.Insert(user);
            }
            else
            {
                user.Teacher = teacher;
                conn.Update(user);
            }
        }
        public void AddOrUpdateURL(string url)
        {
            var user = GetUser();
            if (user == null)
            {
                user = new User { URL = url };
                conn.Insert(user);
            }
            else
            {
                user.URL = url;
                conn.Update(user);
            }
        }

        public void AddOrUpdateTeacherURL(string url)
        {
            var user = GetUser();
            if (user == null)
            {
                user = new User { TeacherURL = url };
                conn.Insert(user);
            }
            else
            {
                user.TeacherURL = url;
                conn.Update(user);
            }
        }
        public void DeleteAll()
        {
            conn.DeleteAll<User>();
        }
       
    }

}
