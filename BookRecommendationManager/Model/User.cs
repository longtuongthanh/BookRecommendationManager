using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRecommendationManager.Model
{
    public partial class User
    {
        #region Functionality
        public User()
        {
            BookListID = new List<string>();
            LikeListID = new List<string>();
            DislikeListID = new List<string>();
        }

        [JsonIgnore]
        private Picture picture;
        public Picture GetPicture()
        {
            if (picture != null && picture.GetImage() != null)
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

        public void AddToBookList(Book book)
        {
            // TODO: change book
            if (!BookListID.Contains(book.Name))
            {
                BookListID.Add(book.Name);
                Database.EditUser();
                book.AddToList++;
                Database.Edit(book);
            }
        }
        public void RemoveFromBookList(Book book)
        {
            // TODO: change book
            if (BookListID.Contains(book.Name))
            {
                BookListID.Remove(book.Name);
                Database.EditUser();
                book.AddToList--;
                Database.Edit(book);
            }
        }
        public void AddToLikeList(Book book)
        {
            bool userChanged = false;
            if (!LikeListID.Contains(book.Name))
            {
                LikeListID.Add(book.Name);
                book.Likes++;
                userChanged = true;
            }
            if (DislikeListID.Contains(book.Name))
            {
                DislikeListID.Remove(book.Name);
                book.Dislike--;
                userChanged = true;
            }
            if (userChanged)
            {
                Database.EditUser();
                Database.Edit(book);
            }
        }
        public void AddToDislikeList(Book book)
        {
            // TODO: change book
            bool userChanged = false;
            if (!DislikeListID.Contains(book.Name))
            {
                DislikeListID.Add(book.Name);
                book.Dislike++;
                userChanged = true;
            }
            if (LikeListID.Contains(book.Name))
            {
                LikeListID.Remove(book.Name);
                book.Likes--;
                userChanged = true;
            }
            if (userChanged)
            {
                Database.EditUser();
                Database.Edit(book);
            }
        }
        public void RemoveFromLikeAndDislikeList(Book book)
        {
            // TODO: change book
            bool userChanged = false;
            if (LikeListID.Contains(book.Name))
            {
                LikeListID.Remove(book.Name);
                book.Likes--;
                userChanged = true;
            }
            if (DislikeListID.Contains(book.Name))
            {
                DislikeListID.Remove(book.Name);
                book.Dislike--;
                userChanged = true;
            }
            if (userChanged)
            {
                Database.EditUser();
                Database.Edit(book);
            }
        }
        #endregion

        #region Data
        public List<string> BookListID { get; set; }
        public List<string> DislikeListID { get; set; }
        public List<string> LikeListID { get; set; }
        // not username
        // not password
        public Int32 Score { get; set; }
        // Score measures overall behaviour & transgressions, including
        // fake book declaration, invalid tags, toxic comment, 
        // and others.
        public string PictureFile { get; set; }

        public string Nickname { get; set; }
        public string Uid { get; set; }

        public DateTime lastActive { get; set; }
        #endregion
    }
}
