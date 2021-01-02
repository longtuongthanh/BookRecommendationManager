using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRecommendationManager.Model
{
    public partial class Book
    {
        #region Functionality
        [JsonIgnore]
        private Picture picture;
        public Picture GetPicture()
        {
            if (picture != null)
                return picture;

            Picture pic = new Picture(PictureFile);
            if (pic.GetImage() == null)
            {
                // Get image from database
                pic.Content = Database.LoadPicture(pic.FilePath);

                // save image to file
                pic.SaveImage();
            }
            return picture = pic;
        }
        public bool IsValid()
        {
            if (Name == null || Name == "")
                return false;
            if (Author == null || Author == "")
                return false;
            if (Description == null || Description == "")
                return false;
            if (Link == null || Link == "")
                return false;
            return true;
        }
        public long GetScore()
        {
            return Database.Setting.AddToListCoefficient * AddToList
                    + Database.Setting.LikeCoefficient * Likes
                    + Database.Setting.ViewCoefficient * Views
                    + InitialScore;
        }
        // Score measures how often the book gets seen, put in 
        // read lists, and likes. Score starts out equal to the
        // score of the user who posted it.
        #endregion
        public Int32 Likes { get; set; }
        public Int32 Dislike { get; set; }
        public Int32 Views { get; set; }
        public Int32 AddToList { get; set; }
        public Int32 InitialScore { get; set; }

        public string Author { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string PictureFile { get; set; }

        public string Link { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public List<Comment> Comment { get; set; } = new List<Comment>();
        public List<string> BaoCao { get; set; } = new List<string>();
    }
}
