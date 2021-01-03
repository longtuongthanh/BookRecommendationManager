using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRecommendationManager.Model
{
    public class Error
    {
        public string UID { get; set; }
        public string ErrorContent { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
