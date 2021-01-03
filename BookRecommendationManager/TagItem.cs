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
    public partial class TagItem : Form
    {
        public string tagName;
        public TagItem(string tagName)
        {
            this.tagName = tagName;
            InitializeComponent();
            label1.Text = tagName;
            button1.Location = new Point(label1.Width + label1.Location.X, button1.Location.Y);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Parent.Controls.Remove(this);
            this.Hide();
            this.Dispose();
        }

        public void label1_SizeChanged(object sender, EventArgs e)
        {
            button1.Location = new Point(label1.Width + label1.Location.X, button1.Location.Y);
        }

        private void TagItem_Resize(object sender, EventArgs e)
        {
            Size = PreferredSize;
        }
    }
}
