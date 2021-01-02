using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BookRecommendationManager.Model;

namespace BookRecommendationManager
{
    public partial class cmt : UserControl
    {
        
        
        public cmt()
        {          
            InitializeComponent();
        }
        public void hienthi(Comment comment)
        {
            User cmtOwner = Database.Users.FirstOrDefault(user => user.Uid == comment.Uid);
            label1.Text = cmtOwner?.Nickname;
            label2.Text = comment.Content;

            Picture pic = cmtOwner?.GetPicture();

            if (pic?.GetImage() != null)
            {
                pictureBox1.Image = pic.GetImage();
            }
            else
                pictureBox1.Image = global::BookRecommendationManager.Properties.Resources.user;
        }
    }
}
