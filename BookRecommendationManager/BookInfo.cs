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
    public partial class BookInfo : Form
    {
        
        private Book currentBook;
        List<Comment> cmtList = new List<Comment>();
        public BookInfo(Book book)
        {
            InitializeComponent();

            deltaDistanceP5_F3 = flowLayoutPanel3.Bottom - panel5.Top;

            currentBook = book;

            labelName.Text = book.Name;
            labelAuthor.Text = "bởi " + book.Author;
            labelDesc.Text = book.Description;
            foreach (var item in book.Tags)
            {
                Label tag = new Label();
                tag.Text = item;
                tag.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
                tag.AutoSize = true;
                flowLayoutPanel1.Controls.Add(tag);
                tag.Show();
            }
            cmtList = book.Comment;
            foreach (var item in book.BaoCao)
            {
                cmt c = new cmt();
                c.hienthi(item);
                flowLayoutPanel2.Controls.Add(c);

            }  
            Picture pic = book.GetPicture();

            if (pic.GetImage() != null)
                picture.Image = pic.GetImage();
            else
                ; // TODO: use default picture
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (!Database.User.LikeListID.Contains(currentBook.Name))
            {
                Database.User.AddToLikeList(currentBook);

                this.pictureBox1.Image = Properties.Resources.Dislike_disabled_;
                this.pictureBox2.Image = Properties.Resources.Like;
            }
            else
            {
                Database.User.RemoveFromLikeAndDislikeList(currentBook);

                this.pictureBox2.Image = Properties.Resources.Like_disabled_;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (!Database.User.DislikeListID.Contains(currentBook.Name))
            {
                Database.User.AddToDislikeList(currentBook);

                this.pictureBox1.Image = Properties.Resources.Dislike;
                this.pictureBox2.Image = Properties.Resources.Like_disabled_;
            }
            else
            {
                Database.User.RemoveFromLikeAndDislikeList(currentBook);

                this.pictureBox1.Image = Properties.Resources.Dislike_disabled_;
            }
        }

        private void panel3_SizeChanged(object sender, EventArgs e)
        {
            labelDesc.MaximumSize = new Size(panel3.Size.Width - 24, 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainMenu owner = Parent.Parent.Parent as MainMenu;
            owner.ClearPanelLoad();

            EditBooks frmEdit = new EditBooks(currentBook) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmEdit.FormBorderStyle = FormBorderStyle.None;
            owner.panelLoad.Controls.Add(frmEdit);
            frmEdit.Show();
        }
        int? deltaDistanceP5_F3 = null;
        void MaintainDistanceOfPanel5AndFlowPanel3()
        {
            if (deltaDistanceP5_F3 != null)
            panel5.Top = flowLayoutPanel3.Bottom - deltaDistanceP5_F3.Value;
        }

        private void flowLayoutPanel3_Resize(object sender, EventArgs e)
        {
            MaintainDistanceOfPanel5AndFlowPanel3();
            labelDesc.MaximumSize = new System.Drawing.Size(flowLayoutPanel3.Width, 0);
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc bạn muốn xóa sách không?", "Xác nhận", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                Database.Delete(currentBook);
                this.Hide();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc bạn muốn xóa tất cả báo cáo không?", "Xác nhận", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                currentBook.BaoCao.Clear();
                Database.Edit(currentBook);

                flowLayoutPanel2.Controls.Clear();
            }
        }
    }
}
