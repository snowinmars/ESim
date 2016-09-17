using Microsoft.Xna.Framework;
using System;

namespace ESim.Entities
{
    /// <summary>
    /// I can't implement IComparer<Color> due to this interface's method return int, but I need double.
    /// </summary>
    public static class ColorComparer
    {
        public static double Compare(Color lhs, Color rhs)
        {
            int dR = lhs.R - rhs.R;
            int dG = lhs.G - rhs.G;
            int dB = lhs.B - rhs.B;

            return Math.Sqrt(dR * dR + dG * dG + dB * dB);
        }
    }
}