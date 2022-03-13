namespace PylonGameEngine.Utilities
{
    public static class Utilities
    {
        public static void Swap<T>(ref T left, ref T right)
        {
            T Buffer = left;
            left = right;
            right = Buffer;
        }
    }
}
