using BookRecommendationManager.Model;
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
    public partial class FormBugDetail : Form
    {
        Error currentError;
        public FormBugDetail(Error error)
        {
            InitializeComponent();
            currentError = error;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa những lỗi này khỏi cơ sở " +
                "dữ liệu không?", "Xác nhận", MessageBoxButtons.YesNo)
                != DialogResult.Yes)
                return;

            Database.Delete(currentError);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private static string GetNicknameOrUID(string UID)
        {
            string result = UID;
            User user = Database.Users.Find(item => item.Uid == UID);
            if (user != null && user.Nickname != null && user.Nickname != "")
                result = user.Nickname;
            return result;
        }

        private void FormBD_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = currentError.Timestamp;

            textBox1.Text = GetNicknameOrUID(currentError.UID);

            richTextBox1.Text = currentError.ErrorContent;
        }
    }
}
