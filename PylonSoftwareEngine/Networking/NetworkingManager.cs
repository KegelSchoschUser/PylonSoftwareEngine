using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PylonSoftwareEngine.Networking
{
    public static class NetworkingManager
    {
        public const int BufferSize = 4096;
        internal static List<(string, Type)> RegisteredPackets = new List<(string, Type)>();

        public static List<Client.Client> Clients = new List<Client.Client>();
        public static List<Server.Server> Servers = new List<Server.Server>();

        public static void RegisterPacket<PacketType>() where PacketType : PacketBase, new()
        {
            Type Type = typeof(PacketType);
            string ID;
            using (var md5 = MD5.Create())
            {
                byte[] Bytes = Encoding.ASCII.GetBytes(Type.Name + Type.GetFields().Length.ToString());
                var hash = md5.ComputeHash(Bytes);
                ID = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }

            RegisteredPackets.Add((ID.ToString(), Type));
        }
    }
}
