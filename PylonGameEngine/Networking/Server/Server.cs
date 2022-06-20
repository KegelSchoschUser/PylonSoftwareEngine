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
    public class Server
    {
        public uint MaxConnections = uint.MaxValue;
        private TcpListener TcpListener;
        internal List<ServerClient> Clients;

        public void Start(int Port)
        {
            Clients = new List<ServerClient>();

            TcpListener = new TcpListener(IPAddress.Any, Port);
            TcpListener.Start();
            TcpListener.BeginAcceptTcpClient(OnClientConnectCallback, null);
        }

        private void OnClientConnectCallback(IAsyncResult result)
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
                var serverclient = new ServerClient(Clients.Count, this);
                Clients.Add(serverclient);
                serverclient.Connect(client);
            }
        }

        public void SendPacketToClient(PacketBase Packet, int Id)
        {
            Clients[Id].SendPacket(Packet);
        }

        public void SendPacketToAll(PacketBase Packet)
        {
            for (int i = 0; i < Clients.Count; i++)
            {
                Clients[i].SendPacket(Packet);
            }
        }

        public void SendPacketToAllExcept(PacketBase Packet, int Id)
        {
            for (int i = 0; i < Clients.Count; i++)
            {
                if (Clients[i].Id != Id)
                    Clients[i].SendPacket(Packet);
            }
        }
    }
}
