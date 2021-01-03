using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookRecommendationManager
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void butExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void butLog_Click(object sender, EventArgs e)
        {
            if (!Firebase.Ins.SignIn(Username.Text, Password.Text))
            {
                DialogResult result =
                        MessageBox.Show(SignUpYesNoPromptContent,
                        SignUpPromptTitle, MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    if (!Firebase.Ins.SignUp(Username.Text, Password.Text))
                    {
                        MessageBox.Show(SignUpFailedPrompt);
                        return;
                    }
                }
                else if (!Firebase.Ins.SignIn(Username.Text, Password.Text))
                {
                    MessageBox.Show(SignInFailedPrompt);
                    return;
                }
            }

            if (!Firebase.Ins.LoadFirebase())
            {
                MessageBox.Show(LoadDataFromFirebaseFailed);
                return;
            }

            #region Check DB code
            Util.StartLoadingForCursor();
            var task = Firebase.Ins.Client.Child("Code").OnceSingleAsync<string>();
            var taskEnd = Task.WhenAny(task, Task.Delay(Firebase.TimeOut));
            taskEnd.Wait();
            if (taskEnd.Result == task)
            {
                try { task.Wait(); } catch
                {
                    MessageBox.Show(LoadDataFromFirebaseFailed);
                    return;
                }

                var taskResult = task.Result;
                if (taskResult != textBox1.Text)
                {
                    MessageBox.Show(WrongDatabaseCode);
                    return;
                }
            }
            else
            {
                MessageBox.Show(LoadDataFromFirebaseFailed);
                return;
            }
            Util.StopLoadingForCursor();
            #endregion

            OpenMainMenu();
        }

        private void OpenMainMenu()
        {
            MainMenu mMenu = new MainMenu();
            mMenu.FormClosing += (obj, arg) => { this.Visible = true; };
            this.Visible = false;
            mMenu.ShowDialog();
        }

        private void butRe_Click(object sender, EventArgs e)
        {
            if (!Firebase.Ins.SignUp(Username.Text, Password.Text))
            {
                MessageBox.Show(SignUpFailedPrompt);
                return;
            }
            if (!Firebase.Ins.LoadFirebase())
            {
                MessageBox.Show(LoadDataFromFirebaseFailed);
                return;
            }
            OpenMainMenu();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        #region Constant
        private const string SignUpFailedPrompt =
            "Đã có lỗi xảy ra. Không đăng kí được.";
        private const string SignUpYesNoPromptContent =
            "Không nhận ra bạn. Bạn có muốn đăng kí?";
        private const string SignUpPromptTitle = "Đăng kí?";
        private const string SignInFailedPrompt
            = "Không thể đăng nhập. Xin thử lại sau ít phút.";
        private const string LoadDataFromFirebaseFailed =
            "Không truy cập được hệ thống dữ liệu.\nXin thử lại sau ít phút.";
        private const string WrongDatabaseCode =
            "Database code sai. Vui lòng liên hệ quản trị database để thêm chi tiết.";
        #endregion

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
