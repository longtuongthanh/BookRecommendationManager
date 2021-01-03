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
    public partial class EditBooks : Form
    {
        Book currentBook;
        List<string> tagList = new List<string>();
        public EditBooks(Book book)
        {
            InitializeComponent();

            currentBook = book;


            textBox1.Text = book.Name;
            textBox2.Text = book.Author;
            textBox3.Text = book.Link;
            richTextBox1.Text = book.Description;
            foreach (string item in book.Tags)
            {
                comboBox1.SelectedItem = item;
                comboBox1_SelectionChangeCommitted(null, null);
            }
            picture.ImageLocation = book.PictureFile;

            comboBox1.DataSource = Database.Tags;

            deltaDistance = flowLayoutPanel1.Bottom - panel1.Top;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Book book = new Book();
            book.Name = textBox1.Text;
            book.Author = textBox2.Text;
            book.Link = textBox3.Text;
            book.Description = richTextBox1.Text;
            book.Tags = tagList;
            
            if (picture.ImageLocation != book.PictureFile)
            {
                Picture pic = new Picture(picture.ImageLocation);
                pic.LoadContent();
                pic.SetNewName();
                book.PictureFile = pic.FilePath;
                Database.Add(pic);
            }

            MessageBox.Show("Sửa thành công");
        }
        /*
        void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            richTextBox1.Text = "";
            this.picture.Image = global::BookRecommendationManager.Properties.Resources._130304;
        }//*/

        private void picture_Click_1(object sender, EventArgs e)
        {
            string imageLocations = "";
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "jpg file(*.jpg)|*.jpg| PNG file(*.png)|*png| All Files(*.*)|*.*";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    imageLocations = dialog.FileName;
                    picture.ImageLocation = imageLocations;
                }
            }
            catch (Exception er)
            {
                MessageBox.Show("An Error Occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Database.PostError(er);
            }
        }

        int deltaDistance;
        void MaintainDistanceOfPanel1AndFlowPanel()
        {
            panel1.Top = flowLayoutPanel1.Bottom - deltaDistance;
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // If the tag is already in the tagList, return.
            if (tagList.Any(item => item == comboBox1.SelectedItem.ToString()))
                return;
            flowLayoutPanel1.PerformLayout();
            TagItem tagItem = new TagItem(comboBox1.SelectedItem.ToString())
            { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };

            tagItem.FormBorderStyle = FormBorderStyle.None;
            flowLayoutPanel1.Controls.Add(tagItem);

            // An empty panel with no content because flow layout panel
            // just don't accept autosizing children.
            Panel minSize = new Panel()
            {
                MinimumSize = new Size(0, 35),
                Size = new Size(0, 35)
            };
            flowLayoutPanel1.Controls.Add(minSize);

            tagItem.Show();
            //flowLayoutPanel1.PerformLayout();

            tagList.Add(comboBox1.SelectedItem.ToString());
            tagItem.SizeChanged += (obj, arg) => flowLayoutPanel1.Invalidate();
        }

        private void flowLayoutPanel1_Resize(object sender, EventArgs e)
        {
            MaintainDistanceOfPanel1AndFlowPanel();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void labelAuthor_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
