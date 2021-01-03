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
            FormCS frmHome = new FormCS() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
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
            ClearPanelLoad();

            FormCS frmHome = new FormCS() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmHome.FormBorderStyle = FormBorderStyle.None;
            this.panelLoad.Controls.Add(frmHome);
            frmHome.Show();
        }

        private void butMybooks_Click(object sender, EventArgs e)
        {
            ClearPanelLoad();

            FormCatalog frmBug = new FormCatalog() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmBug.FormBorderStyle = FormBorderStyle.None;
            this.panelLoad.Controls.Add(frmBug);
            frmBug.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearPanelLoad();

            FormBug frmBug = new FormBug() { Dock = DockStyle.Fill, TopLevel = false, TopMost = true };
            frmBug.FormBorderStyle = FormBorderStyle.None;
            this.panelLoad.Controls.Add(frmBug);
            frmBug.Show();
        }

        public void ClearPanelLoad()
        {
            foreach (Control item in this.panelLoad.Controls)
            {
                if (item is Form)
                    (item as Form).Hide();
                else
                    item.Dispose();
            }

            this.panelLoad.Controls.Clear();
        }
    }
}
