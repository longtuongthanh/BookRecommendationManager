using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookRecommendationManager.Model
{
    public partial class Database
    {
        #region Database
        private static List<Book> s_books;
        private static Setting s_setting;
        private static List<string> s_tags;
        private static User s_user;
        private static List<User> s_users;
        private static List<Error> s_errors;

        static public List<Book> Books
        {
            get => s_books;
            set => s_books = value;
        }
        static public List<string> Tags
        {
            get => s_tags;
            set => s_tags = value;
        }
        static public User User
        {
            get => s_user;
            set => s_user = value;
        }
        static public List<User> Users
        {
            get => s_users;
            set => s_users = value;
        }
        static public Setting Setting
        {
            get => s_setting;
            set => s_setting = value;
        }
        public static List<Error> Errors
        {
            get => s_errors;
            set => s_errors = value;
        }
        #endregion

        #region Functionality
        static public void Add(Book book)
        {
            Util.StartLoadingForCursor();
            try
            {
                if (book.IsValid())
                {
                    Firebase.Ins.Client.Child("Books").Child(book.Name).PutAsync(JsonConvert.SerializeObject(book)).Wait();
                    Books.Add(book);
                    UserActive();
                }
                else
                {
                    PostError("ERROR: book name is null \nAt Database::Add(book) with current Book: " +
                        JsonConvert.SerializeObject(book));
                }
            }
            catch (Exception e)
            {
                PostError(e);
            }
            Util.StopLoadingForCursor();
        }
        static public void Delete(Book book)
        {
            Util.StartLoadingForCursor();
            try
            {
                if (book.IsValid())
                {
                    Firebase.Ins.Client.Child("Books").Child(book.Name).DeleteAsync().Wait();
                    Books.Remove(book);
                    UserActive();
                }
                else
                {
                    PostError("ERROR: book name is null \nAt Database::Delete(book) with current Book: " +
                        JsonConvert.SerializeObject(book));
                }
            }
            catch (Exception e)
            {
                PostError(e);
            }
            Util.StopLoadingForCursor();
        }
        static public void Edit(Book book)
        {
            Add(book);
        }
        static public void Add(Picture pic)
        {
            Util.StartLoadingForCursor();
            try
            {
                if (pic.FilePath == null || pic.Content == null ||
                    pic.FilePath == "" || pic.Content == "")
                {
                    PostError("ERROR: invalid picture \nAt Database::Add(Picture) with current Picture: " +
                        JsonConvert.SerializeObject(pic));
                    return;
                }

                string FilePath = pic.FilePath.Replace(".", ",");
                Firebase.Ins.Client.Child("Picture").Child(FilePath).PutAsync(JsonConvert.SerializeObject(pic.Content)).Wait();
            }
            catch (Exception e)
            {
                PostError(e);
            }
            Util.StopLoadingForCursor();
        }
        static public void UserActive()
        {
            Util.StartLoadingForCursor();
            try
            {
                string uid = Firebase.Ins.Token.User.LocalId;
                if (uid != null || uid == "")
                    Firebase.Ins.Client.Child("Users").Child(uid).Child("lastActive").PutAsync(DateTime.Now).Wait();
                else
                {
                    PostError("ERROR: UID is null \nAt Database::EditUser() with current User: " +
                        JsonConvert.SerializeObject(User));
                }
            }
            catch (Exception e)
            {
                PostError(e);
            }
            Util.StopLoadingForCursor();
        }

        static public void EditUser()
        {
            Util.StartLoadingForCursor();
            try
            {
                string uid = Firebase.Ins.Token.User.LocalId;
                User.lastActive = DateTime.Now;
                User.Uid = uid;
                if (uid != null || uid == "")
                    Firebase.Ins.Client.Child("Users").Child(uid).PutAsync(User).Wait();
                else
                {
                    PostError("ERROR: UID is null \nAt Database::EditUser() with current User: " + 
                        JsonConvert.SerializeObject(User));
                }
            }
            catch (Exception e)
            {
                PostError(e);
            }
            Util.StopLoadingForCursor();
        }
        static public void Add(string tag)
        {
            Util.StartLoadingForCursor();
            try
            {
                if (tag != null && tag != "")
                    Firebase.Ins.Client.Child("Tags").PostAsync(tag);
                Tags.Add(tag);
            }
            catch (Exception e)
            {
                PostError(e);
            }
            Util.StopLoadingForCursor();
        }
        static public string LoadPicture(string FilePath)
        {
            string result;
            Util.StartLoadingForCursor();
            try
            {
                result = Firebase.Ins.LoadPicture(FilePath);
            }
            catch (Exception e)
            {
                PostError(e);
                result = null;
            }
            Util.StopLoadingForCursor();
            return result;
        }
        static public void PostError(Exception e)
        {
            Util.StartLoadingForCursor();
            try
            {
                if (e != null)
                {
                    string uid = Firebase.Ins.Token.User.LocalId;
                    if (uid != null || uid == "")
                        Firebase.Ins.Client.Child("Error").Child(uid + DateTime.Now.ToString()).PutAsync(e.ToString());
                    else Console.WriteLine("ERROR: UID is null");
                }
            }
            catch (Exception e2)
            {
                Console.WriteLine("ERROR: cannot post error to database. Current error: " + 
                    e2.ToString() + "\nTarget Error: " + e.ToString());
            }
            Util.StopLoadingForCursor();
        }
        static public void PostError(string e)
        {
            Util.StartLoadingForCursor();
            try
            {
                if (e != null)
                {
                    string uid = Firebase.Ins.Token.User.LocalId;
                    if (uid != null || uid == "")
                        Firebase.Ins.Client.Child("Error").Child(uid + DateTime.Now.ToString()).PutAsync(e);
                    else Console.WriteLine("ERROR: UID is null");
                }
            }
            catch (Exception e2)
            {
                Console.WriteLine("ERROR: cannot post error to database. Current error: " +
                    e2.ToString() + "\nTarget Error: " + e);
            }
            Util.StopLoadingForCursor();
        }
        #endregion
    }
}
