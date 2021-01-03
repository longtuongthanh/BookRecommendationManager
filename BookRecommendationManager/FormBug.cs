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
    public partial class FormBug : Form
    {
        public FormBug()
        {
            InitializeComponent();
        }

        private void FormBug_Load(object sender, EventArgs e)
        {
            LoadBugForm();
        }

        public static string GetNicknameOrUID(string UID)
        {
            string result = UID;
            User user = Database.Users.Find(item => item.Uid == UID);
            if (user != null && user.Nickname != null && user.Nickname != "")
                result = user.Nickname;
            return result;
        }

        private void LoadBugForm()
        {
            var ErrorBindingSource = new BindingSource();
            ErrorBindingSource.DataSource = Database.Errors.ConvertAll(item =>
                new Error {
                    Timestamp = item.Timestamp,
                    ErrorContent = item.ErrorContent,
                    UID = GetNicknameOrUID(item.UID)
                });
            dataGridView1.DataSource = ErrorBindingSource;
            dataGridView1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1)
            {
                MessageBox.Show("Vui lòng chọn 1 lỗi duy nhất");
                return;
            }

            Error error = dataGridView1.SelectedRows[0].DataBoundItem as Error;

            FormBugDetail bugDetail = new FormBugDetail(error);
            bugDetail.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa những lỗi này khỏi cơ sở " +
                "dữ liệu không?", "Xác nhận", MessageBoxButtons.YesNo)
                != DialogResult.Yes)
                return;

            Error error = dataGridView1.SelectedRows[0].Tag as Error;

            Database.Delete(error);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa tất cả lỗi khỏi cơ sở " +
                "dữ liệu không?", "Xác nhận", MessageBoxButtons.YesNo)
                != DialogResult.Yes)
                return;
        }
    }
}
