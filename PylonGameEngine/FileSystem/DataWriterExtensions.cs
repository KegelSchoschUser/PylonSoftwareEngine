using PylonGameEngine.Mathematics;

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
