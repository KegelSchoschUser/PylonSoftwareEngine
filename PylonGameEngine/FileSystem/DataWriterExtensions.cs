using PylonGameEngine.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PylonGameEngine.FileSystem
{
    public static class DataWriterExtensions
    {
        public static void WriteRGBColor(this DataWriter writer, RGBColor value)
        {
            writer.WriteFloat(value.R);
            writer.WriteFloat(value.G);
            writer.WriteFloat(value.B);
            writer.WriteFloat(value.A);
        }
    }
}
