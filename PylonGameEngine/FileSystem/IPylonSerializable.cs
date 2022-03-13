namespace PylonGameEngine.FileSystem
{
    public interface IPylonSerializable
    {
        public bool Serialize(DataWriter writer);
        public bool DeSerialize(DataReader reader);
    }
}
