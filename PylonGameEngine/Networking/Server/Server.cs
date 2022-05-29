using PylonGameEngine.FileSystem.DataSources;
using PylonGameEngine.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using PylonGameEngine.Utilities;
using System.Net;

namespace PylonGameEngine.Networking.Server
{
    public static class Server
    {
        public static uint MaxConnections = uint.MaxValue;
        private static TcpListener TcpListener;
        internal static List<ServerClient> Clients;

        public static void Start(int Port)
        {
            Clients = new List<ServerClient>();

            TcpListener = new TcpListener(IPAddress.Any, Port);
            TcpListener.Start();
            TcpListener.BeginAcceptTcpClient(OnClientConnectCallback, null);
        }

        private static void OnClientConnectCallback(IAsyncResult result)
        {
            TcpClient client = TcpListener.EndAcceptTcpClient(result);
            TcpListener.BeginAcceptTcpClient(OnClientConnectCallback, null);

            MyLog.Default.Write($"Incoming connection from {client.Client.RemoteEndPoint}...");

            if(Clients.Count >= MaxConnections)
            {
                MyLog.Default.Write($"{client.Client.RemoteEndPoint} failed to connect: Server full!");
            }
            else
            {
                var serverclient = new ServerClient(Guid.NewGuid().ToString());
                Clients.Add(serverclient);
                serverclient.Connect(client);
            }
        }

        public static void SendPacketToClient(PacketBase Packet, int Index)
        {
            Clients[Index].SendPacket(Packet);
        }

        public static void SendPacketToClient(PacketBase Packet, string Id)
        {
            Clients.Find(x => x.Id == Id).SendPacket(Packet);
        }

        public static void SendPacketToAll(PacketBase Packet)
        {
            for (int i = 0; i < Clients.Count; i++)
            {
                Clients[i].SendPacket(Packet);
            }
        }

        public static void SendPacketToAllExcept(PacketBase Packet, int Index)
        {
            for (int i = 0; i < Clients.Count; i++)
            {
                if(i != Index)
                    Clients[i].SendPacket(Packet);
            }
        }

        public static void SendPacketToAllExcept(PacketBase Packet, string Id)
        {
            for (int i = 0; i < Clients.Count; i++)
            {
                if (Clients[i].Id != Id)
                    Clients[i].SendPacket(Packet);
            }
        }
    }
}
