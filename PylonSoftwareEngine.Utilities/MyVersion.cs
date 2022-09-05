namespace PylonSoftwareEngine.Utilities
{
    public class MyVersion
    {
        public int Major { get; private set; }
        public int Minor { get; private set; }
        public int Patch { get; private set; }
        public int Build { get; private set; }

        public MyVersion(int major, int minor, int minorbuild, int build = -1)
        {
            Major = major;
            Minor = minor;
            Patch = minorbuild;
            Build = build;
        }

        public override string ToString()
        {
            return Major + "." + Minor + "." + Patch + ":" + Build;
        }
    }
}
