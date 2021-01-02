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
    public partial class FormCS : Form
    {
        public FormCS()
        {
            InitializeComponent();
        }

        private void FormCS_Load(object sender, EventArgs e)
        {
            int bookCount = Database.Books.Count;
            label5.Text = Database.Books.Count.ToString();
            label6.Text = Database.Books.Count(
                item => item.Comment != null && item.Comment.Count > 0
                ).ToString();
            label7.Text = Database.Errors.Count.ToString();

            for (int i = 0; i< Database.Tags.Count; i++)
            {
                string tag = Database.Tags[i];
                dataGridView1.Rows.Insert(i, tag,
                    (Database.Books.Count(
                        item => item.Tags.Contains(tag)
                        ) / (double) bookCount * 100).ToString() + " %");
            }
        }
    }
}
