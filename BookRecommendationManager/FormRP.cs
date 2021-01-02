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
    public partial class FormRP : Form
    {
        Book currentBook;
        public FormRP(Book book)
        {
            InitializeComponent();
            currentBook = book;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                currentBook.BaoCao.Add(checkBox1.Text);
            if (checkBox2.Checked)
                currentBook.BaoCao.Add(checkBox2.Text);
            if (checkBox3.Checked)
                currentBook.BaoCao.Add(checkBox3.Text);

            Database.Edit(currentBook);

            MessageBox.Show("Đã gửi báo cáo!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
