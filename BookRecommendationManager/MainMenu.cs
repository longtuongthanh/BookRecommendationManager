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
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
            FormHome frmHome = new FormHome() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmHome.FormBorderStyle = FormBorderStyle.None;
            this.panelLoad.Controls.Add(frmHome);
            frmHome.Show();
        }
  

        private void butExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void butMax_Click_1(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                WindowState = FormWindowState.Maximized;
            else
                WindowState = FormWindowState.Normal;
        }

        private void butMin_Click_1(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;

        }

        private void butHome_Click(object sender, EventArgs e)
        {
            foreach (Control item in this.panelLoad.Controls)
                item.Dispose();

            this.panelLoad.Controls.Clear();
            FormHome frmHome = new FormHome() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmHome.FormBorderStyle = FormBorderStyle.None;
            this.panelLoad.Controls.Add(frmHome);
            frmHome.Show();
        }

        private void butMybooks_Click(object sender, EventArgs e)
        {
            foreach (Control item in this.panelLoad.Controls)
                item.Dispose();

            this.panelLoad.Controls.Clear();

            FormMyBooks frmMB = new FormMyBooks() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmMB.FormBorderStyle = FormBorderStyle.None;
            this.panelLoad.Controls.Add(frmMB);
            frmMB.Show();
        }           
        private void butAcc_Click(object sender, EventArgs e)
        {
            foreach (Control item in this.panelLoad.Controls)
                item.Dispose();

            this.panelLoad.Controls.Clear();

            FormAcc frmAcc = new FormAcc() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmAcc.FormBorderStyle = FormBorderStyle.None;
            this.panelLoad.Controls.Add(frmAcc);
            frmAcc.Show();
        }

        private void butHelp_Click(object sender, EventArgs e)
        {
            foreach (Control item in this.panelLoad.Controls)
                item.Dispose();

            this.panelLoad.Controls.Clear();

            FormHelp frmHelp = new FormHelp() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmHelp.FormBorderStyle = FormBorderStyle.None;
            this.panelLoad.Controls.Add(frmHelp);
            frmHelp.Show();
        }


        private void butSearch_Click(object sender, EventArgs e)
        {
            foreach (Control item in this.panelLoad.Controls)
                item.Dispose();

            this.panelLoad.Controls.Clear();

            frmSearch frmSearch = new frmSearch() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmSearch.FormBorderStyle = FormBorderStyle.None;
            frmSearch.SearchCriteria = textBox1.Text;
            this.panelLoad.Controls.Add(frmSearch);
            frmSearch.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Control item in this.panelLoad.Controls)
                item.Dispose();

            this.panelLoad.Controls.Clear();

            AddBooks frmadd = new AddBooks { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmadd.FormBorderStyle = FormBorderStyle.None;
            this.panelLoad.Controls.Add(frmadd);
            frmadd.Show();
        }
    }
}
