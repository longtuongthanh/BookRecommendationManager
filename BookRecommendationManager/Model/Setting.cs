using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookRecommendationManager.Model
{
    public partial class Setting
    {
        public Int32 LikeCoefficient { get; set; }
        public Int32 ViewCoefficient { get; set; }
        public Int32 AddToListCoefficient { get; set; }

        public Int32 SearchAuthorMatchCoefficient { get; set; }
        public Int32 SearchNameMatchCoefficient { get; set; }
        public Int32 SearchTagMatchCoefficient { get; set; }
        public Int32 SearchDescriptionMatchCoefficient { get; set; }
        public Int32 SearchNotMatchPenalty { get; set; }

        public Int32 SearchMaxResultAmount { get; set; }
    }
}
