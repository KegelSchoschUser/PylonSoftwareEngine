using PylonGameEngine.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PylonGameEngine.FileSystem
{
    public static class DataReaderExtensions
    {
        public static RGBColor ReadRGBColor(this DataReader reader)
        {
            RGBColor color = new RGBColor();
            color.R = reader.ReadFloat();
            color.G = reader.ReadFloat();
            color.B = reader.ReadFloat();
            color.A = reader.ReadFloat();

            return color;
        }
    }
}
