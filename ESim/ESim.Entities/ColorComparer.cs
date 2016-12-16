using Microsoft.Xna.Framework;
using System;
using Colourful;
using Colourful.Conversion;
using Colourful.Difference;

namespace ESim.Entities
{
    /// <summary>
    /// I can't implement IComparer<Color> due to this interface's method return int, but I need double.
    /// </summary>
    public static class ColorComparer
    {
        private static readonly CIEDE2000ColorDifference CIEDE2000ColorDifference = new CIEDE2000ColorDifference();
        private static readonly ColourfulConverter Converter = new ColourfulConverter {WhitePoint = Illuminants.D65};

        public static double Compare(Color lhs, Color rhs)
        {
            RGBColor lRGBcolor = new RGBColor(lhs.R/(double) 255,
                lhs.G/(double) 255,
                lhs.B/(double) 255);
            RGBColor rRGBcolor = new RGBColor(rhs.R/(double) 255,
                rhs.G/(double) 255,
                rhs.B/(double) 255);

            LabColor lLABcolor = ColorComparer.Converter.ToLab(lRGBcolor);
            LabColor rLABcolor = ColorComparer.Converter.ToLab(rRGBcolor);


            return ColorComparer.CIEDE2000ColorDifference.ComputeDifference(lLABcolor, rLABcolor);
        }
    }
}