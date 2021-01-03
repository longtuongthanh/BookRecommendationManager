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
    public partial class FormCatalog : Form
    {
        Action<Book> refreshOnUpdate;
        public FormCatalog()
        {
            InitializeComponent();
            LoadBooks();
        }
        public void LoadBooks ()
        {
            IEnumerable<Book> reportedBook = Database.Books.Where(
                item => item.BaoCao != null && item.BaoCao.Count > 0);
            foreach (Book item in reportedBook)
            {


                Panel pal = new Panel()
                {
                    Width = 350,
                    Height = 230
                };


                ApplyBookItem(pal, item);
                flowLayoutPanel1.Controls.Add(pal);
                
            }
            if (reportedBook.Count() > 0)
            {
                label1.Hide();
            }
        }
        public void ClearBooks()
        {
            flowLayoutPanel1.Controls.Clear();
            label1.Show();
        }
        public void ApplyBookItem(Panel panel, Book book)
        {
            BookItem frmBI = new BookItem(
                book, SelectedBook)
            { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmBI.FormBorderStyle = FormBorderStyle.None;
            panel.Controls.Add(frmBI);
            frmBI.Show();
        }

        private void SelectedBook(object sender, EventArgs e)
        {
            MainMenu owner = Parent.Parent.Parent as MainMenu;
            owner.ClearPanelLoad();

            BookInfo frmBI = new BookInfo(sender as Book) { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmBI.FormBorderStyle = FormBorderStyle.None;
            owner.panelLoad.Controls.Add(frmBI);
            frmBI.Show();
        }
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FormHome_FormClosing(object sender, FormClosingEventArgs e)
        {
            Firebase.Ins.onBookUpdate -= refreshOnUpdate;
        }

        private void FormHome_Load(object sender, EventArgs e)
        {
            refreshOnUpdate = (book) =>
            {
                Action action = () =>
                {
                    ClearBooks();
                    LoadBooks();
                };
                // Cross-thread action
                this.Invoke(action);
            };
            Firebase.Ins.onBookUpdate += refreshOnUpdate;
        }
    }
}
