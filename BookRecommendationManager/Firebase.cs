using BookRecommendationManager.Model;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRecommendationManager
{
    public class Firebase
    {
        #region Singleton
        public static Firebase Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new Firebase();
                return _ins;
            }
        }

        private static Firebase _ins;
        private Firebase()
        {
            authProvider = new FirebaseAuthProvider(new FirebaseConfig(firebaseApiKey));
        }
        #endregion

        #region Constant
        public const int TimeOut = 10000;
        #endregion

        #region FirebaseSetting
        // TODO: export to file
        // TODO: encrypt said file
        private string firebaseApiKey = BookRecommendationManager.Properties.Settings.Default.firebaseApiKey;
        // Get at Setting->Cloud Messaging->Server Key
        private string databaseURL = BookRecommendationManager.Properties.Settings.Default.databaseURL;
        // Get at Realtime Database
        private FirebaseAuthProvider authProvider;
        private FirebaseClient client = null;
        private FirebaseAuthLink token = null;
        private Task refreshToken;

        public FirebaseAuthLink Token
        {
            get => token;
            set
            {
                token = value;
                Client = new FirebaseClient(databaseURL,
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(Token.FirebaseToken)
                });
            }
        }

        public FirebaseClient Client { get => client; set => client = value; }
        #endregion

        #region LoadData
        public bool SignUp(string email, string password)
        {
            Util.StartLoadingForCursor();
            var authActionSignUp = authProvider.CreateUserWithEmailAndPasswordAsync(email, password);
            bool error = false;
            try { authActionSignUp.Wait(TimeOut); }
            catch { error = true; }
            if (authActionSignUp.IsFaulted || error || !authActionSignUp.IsCompleted)
            {
                //MessageBox.Show(SignUpFailedPrompt);
                Util.StopLoadingForCursor();
                return false;
            }
            Token = authActionSignUp.Result;

            Client.Child("Users").Child(Token.User.LocalId).PutAsync(new Model.User
            {
                BookListID = new List<string>(),
                Score = 0,
                Uid = Token.User.LocalId
            }).Wait();
            Util.StopLoadingForCursor();
            return true;
        }

        public bool SignIn(string email, string password)
        {
            Util.StartLoadingForCursor();
            var authActionSignIn = authProvider.SignInWithEmailAndPasswordAsync(email, password);
            bool error = false;
            try { authActionSignIn.Wait(TimeOut); }
            catch { error = true; }
            if (authActionSignIn.IsFaulted || error || !authActionSignIn.IsCompleted)
            {
                Util.StopLoadingForCursor();
                return false;
                /*
                if (triedOnce)
                {
                    MessageBox.Show(SignInFailedPrompt);
                    return null;
                }
                else
                {
                    DialogResult result =
                        MessageBox.Show(SignUpYesNoPromptContent,
                        SignUpPromptTitle, MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        try { SignUp(email, password); }
                        catch
                        {
                            MessageBox.Show(SignUpFailedPrompt);
                            return null;
                        }
                        return SignIn(email, password);
                    }
                    else return null;
                }
                //*/
            }
            Token = authActionSignIn.Result;
            Util.StopLoadingForCursor();
            return true;
        }

        #region Tasks
        public event Action<Book> onBookUpdate;
        public IDisposable subscribeBook;
        private bool LoadBook()
        {
            var query = Client.Child("Books");

            var task = query.OnceAsync<Book>();
            var taskEnd = Task.WhenAny(task, Task.Delay(TimeOut));
            taskEnd.Wait();
            if (taskEnd.Result == task)
            {
                try { task.Wait(); } catch { return false; }
                var taskResult = task.Result;

                Database.Books = taskResult.Select(item => item.Object).ToList();

                subscribeBook = query.AsObservable<Book>().Subscribe(ev =>
                {
                    try
                    {
                        Book book = ev.Object;
                        Database.Books.RemoveAll(item => book.Name == item.Name);
                        Database.Books.Add(book);

                        onBookUpdate?.Invoke(book);
                    }
                    catch (Exception e) {; }
                },
                (Exception er) => { Console.WriteLine(er.ToString()); }, // If error
                () => { Console.WriteLine("Xong"); }); // When done

                return !task.IsFaulted;
            }
            else return false;
        }
        public event Action onTagUpdate;
        public IDisposable subscribeTag;
        private bool LoadTags()
        {
            var query = Client.Child("Tags");

            var task = query.OnceSingleAsync<List<string>>();
            var taskEnd = Task.WhenAny(task, Task.Delay(TimeOut));
            taskEnd.Wait();
            if (taskEnd.Result == task)
            {
                try { task.Wait(); } catch { return false; }
                Database.Tags = task.Result;

                //*
                subscribeTag = query.AsObservable<string>().Subscribe(ev => 
                {
                    string tag = ev.Object;
                    Database.Tags.RemoveAll(item => item == tag);
                    Database.Tags.Add(tag);

                    onTagUpdate?.Invoke();
                });//*/

                return !task.IsFaulted;
            }
            else return false;
        }
        private bool LoadUser()
        {
            var task = Client.Child("Users").Child(Token.User.LocalId).OnceSingleAsync<Model.User>();
            var taskEnd = Task.WhenAny(task, Task.Delay(TimeOut));
            taskEnd.Wait();
            if (taskEnd.Result == task)
            {
                try { task.Wait(); } catch { return false; }
                Database.User = task.Result;
                return !task.IsFaulted;
            }
            else return false;
        }
        private bool LoadUsers()
        {
            var task = Client.Child("Users").OnceAsync<Model.User>();
            var taskEnd = Task.WhenAny(task, Task.Delay(TimeOut));
            taskEnd.Wait();
            if (taskEnd.Result == task)
            {
                try { task.Wait(); } catch { return false; }
                var taskResult = task.Result;
                Database.Users = taskResult.Select(item => item.Object).ToList();
                return !task.IsFaulted;
            }
            else return false;
        }
        private bool LoadSetting()
        {
            var task = Client.Child("Setting").OnceSingleAsync<Setting>();
            var taskEnd = Task.WhenAny(task, Task.Delay(TimeOut));
            taskEnd.Wait();
            if (taskEnd.Result == task)
            {
                try { task.Wait(); } catch { return false; }
                Database.Setting = task.Result;
                return !task.IsFaulted;
            }
            else return false;
        }
        private bool LoadError()
        {
            var task = Client.Child("Error").OnceAsync<string>();
            var taskEnd = Task.WhenAny(task, Task.Delay(TimeOut));
            taskEnd.Wait();
            if (taskEnd.Result == task)
            {
                try { task.Wait(); } catch { return false; }

                var taskResult = task.Result;
                Database.Errors = taskResult.Select(
                    item => new Error { ErrorContent = item.Object, UID = item.Key }
                    ).ToList();
                return !task.IsFaulted;
            }
            else return false;
        }
        // Returns Picture.Content
        public string LoadPicture(string FilePath)
        {
            // Firebase doesn't accept '.'
            if (FilePath == null) return null;
            FilePath = FilePath.Replace(".", ",");

            var task = Client.Child("Picture").Child(FilePath).OnceSingleAsync<string>();
            var taskEnd = Task.WhenAny(task, Task.Delay(TimeOut));
            taskEnd.Wait();
            if (taskEnd.Result == task)
            {
                try { task.Wait(); } catch { return null; }
                if (task.IsFaulted) return null;
                return task.Result;
            }
            else return null;
        }
        #endregion

        #region Refresh Token
        public async Task RefreshToken()
        {
            // Non-blocking thread, so infinite loop is ok
            while (true)
            {
                // Yield control if Token not expired.
                await Task.Delay(Token.ExpiresIn);
                var task = Token.GetFreshAuthAsync();
                try { task.Wait(); } catch
                {
                    Console.WriteLine("WARNING: cannot refresh token");
                    continue;
                }
                Token = task.Result;
            }
        }
        #endregion

        public bool LoadFirebase(string username = null, string password = null)
        {
            Util.StartLoadingForCursor();
            bool result = true;
            if (Client == null)
                result = SignIn(username, password);
            if ((Client == null) || (result == false))
                return false;

            refreshToken = RefreshToken();
            result &= LoadBook();
            result &= LoadTags();
            result &= LoadUser();
            result &= LoadUsers();
            result &= LoadSetting();
            result &= LoadError();
            /*
            if (result == false)
                MessageBox.Show(LoadDataFromFirebaseFailed);
            //*/

            Database.UserActive();

            Util.StopLoadingForCursor();
            return result;
        }
        #endregion

        #region CloseFirebase
        public void CloseFirebase()
        {
            Client.Dispose();
            refreshToken.Dispose();
            Client = null;
            Token = null;
            refreshToken = null;
        }
        #endregion
    }
}
