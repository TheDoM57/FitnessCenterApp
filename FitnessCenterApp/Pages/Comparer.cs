using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessCenterApp.Model;


namespace FitnessCenterApp.Pages
{
    class Comparer
    {
        public static int CompareSpecialization(Specialization x, Specialization y)
        {
            int xId = x.Id, yId = y.Id;
            int xScore = (xId & 1) + ((xId & 2) >> 1) + ((xId & 4) >> 2) + ((xId & 8) >> 3);
            int yScore = (yId & 1) + ((yId & 2) >> 1) + ((yId & 4) >> 2) + ((yId & 8) >> 3);
            int result = xScore - yScore;
            if (result == 0)
                return xId - yId;
            return result;
        }
    }
}
