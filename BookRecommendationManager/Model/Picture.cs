using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRecommendationManager.Model
{
    // WARNING: DO NOT SERIALIZE & PUSH TO DATABASE
    public class Picture : IDisposable
    {
        private Image image = null;

        public Picture(string filepath) { FilePath = filepath; }

        public string FilePath { get; set; }
        public string Content { get; set; }
        
        public void SetNewName()
        {
            if (FilePath == null)
                return;

            string newName = Guid.NewGuid().ToString();
            string type = FilePath.Split('.').Last();
            FilePath = newName + '.' + type;
        }

        public Image GetImage()
        {
            if (image != null)
                return image;

            // Invalid Picture object
            if (FilePath == null)
                return null;

            // Make picture from hash
            if (Content != null)
            {
                using (FileStream cout = File.OpenWrite(FilePath))
                {
                    byte[] data = Util.Decrypt(Content);
                    cout.Write(data, 0, data.Length);
                }
                return image = Image.FromFile(FilePath);
            }
            else // Get picture from file
            {
                try
                {
                    return image = Image.FromFile(FilePath);
                }
                catch (Exception e) {
                    if (e is OutOfMemoryException)
                        throw e;
                    Database.PostError(e);
                    // Outside, try to get hash from database
                    return null;
                }
            }
        }

        // Save to Computer
        public void SaveImage()
        {
            // if Content (provided outside) is not available
            if (Content == null)
                return;

            // Invalid Picture object
            if (FilePath == null)
                return;
            
            using (FileStream cout = File.OpenWrite(FilePath))
            {
                byte[] data = Util.Decrypt(Content);
                cout.Write(data, 0, data.Length);
            }
        }

        // Load from Computer
        public void LoadContent()
        {
            // Invalid Picture object
            if (FilePath == null)
                return;

            using (FileStream cin = File.OpenRead(FilePath))
            {
                byte[] data = new byte[cin.Length];
                cin.Read(data, 0, data.Length);

                Content = Util.Encrypt(data);
            }
        }

        public void Dispose()
        {
            image?.Dispose();
        }
    }
}
