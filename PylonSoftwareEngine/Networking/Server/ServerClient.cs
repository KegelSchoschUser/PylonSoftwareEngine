/*!
 * PylonSoftwareEngine - C# Library for creating Software/Games with DirectX (11)
 * https://github.com/PylonDev/PylonSoftwareEngine
 * Copyright (C) 2022 Endric Barnekow <pylon@pylonmediagroup.de>
 * https://github.com/PylonDev/PylonSoftwareEngine/blob/master/LICENSE.md
 */

using PylonSoftwareEngine.FileSystem.DataSources;
using PylonSoftwareEngine.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using PylonSoftwareEngine.Utilities;

namespace PylonSoftwareEngine.Networking.Server
{
    public class ServerClient
    {
        private NetworkStream Stream;
        private byte[] ReceiveBuffer;
        private TcpClient TcpClient;
        public int Id { get; private set; }
        private Server Server;

        public ServerClient(int id, Server server)
        {
            Id = id;
            Server = server;
        }

        public void Connect(TcpClient tcpClient)
        {
            TcpClient = tcpClient;
            TcpClient.ReceiveBufferSize = NetworkingManager.BufferSize;
            TcpClient.SendBufferSize = NetworkingManager.BufferSize;

            Stream = TcpClient.GetStream();

            ReceiveBuffer = new byte[NetworkingManager.BufferSize];

            Stream.BeginRead(ReceiveBuffer, 0, NetworkingManager.BufferSize, ReceiveCallback, null);

           //Welcome Message event (Server > Client)
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int length = Stream.EndRead(_result);
                if (length <= 0)
                {
                    MyLog.Default.Write("TcpClient Disconnect: Stream size was 0.", LogSeverity.Warning);
                    Disconnect();
                    return;
                }

                ByteArraySource data = new ByteArraySource();
                data.Data.AddRange(ReceiveBuffer);

                HandleData(data);

                Stream.BeginRead(ReceiveBuffer, 0, NetworkingManager.BufferSize, ReceiveCallback, null);
            }
            catch (Exception ex)
            {
                MyLog.Default.Write("TcpClient Disconnect: Something happened in 'ReceiveCallback'", LogSeverity.Warning);
                MyLog.Default.Write(ex, LogSeverity.Warning);
                Disconnect();
            }
        }

        private void HandleData(ByteArraySource data)
        {
            DataReader dataReader = new DataReader(data);

            string PacketID = dataReader.ReadString();

            var PacketType = NetworkingManager.RegisteredPackets.Find(x => x.Item1 == PacketID).Item2;
            var Packet = (PacketBase)Activator.CreateInstance(PacketType);

            Packet.OnPacketReceived(dataReader, Id);
        }

        public void SendPacket(PacketBase Packet)
        {
            try
            {
                if (TcpClient == null)
                {
                    MyLog.Default.Write("Error sending Packet to client because the Client was not initialized.", LogSeverity.Warning);
                }

                ByteArraySource data = new ByteArraySource();
                var dataWriter = new DataWriter(data);
                Type type = Packet.GetType();

                dataWriter.WriteString(NetworkingManager.RegisteredPackets.Find(x => x.Item2 == type).Item1);
                dataWriter.WriteBytes(Packet.Data.Data.ToArray());

                Stream.BeginWrite(data.Data.ToArray(), 0, data.Data.Count, null, null);
            }
            catch (Exception ex)
            {
                MyLog.Default.Write("Error sending Packet to client", LogSeverity.Warning);
                MyLog.Default.Write(ex);
            }
        }

        internal void Disconnect()
        {
            TcpClient.Close();
            MyLog.Default.Write("TcpClient Disconnect", LogSeverity.Info);
            Server.Clients.Remove(this);
        }
    }
}
