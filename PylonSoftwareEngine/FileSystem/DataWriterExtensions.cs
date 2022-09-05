using PylonSoftwareEngine.Mathematics;

namespace PylonSoftwareEngine.FileSystem
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
