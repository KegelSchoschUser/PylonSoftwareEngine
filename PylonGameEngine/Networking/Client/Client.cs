using PylonGameEngine.FileSystem.DataSources;
using PylonGameEngine.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using PylonGameEngine.Utilities;

namespace PylonGameEngine.Networking.Client
{
    public static class Client
    {
        private static NetworkStream Stream;
        private static byte[] ReceiveBuffer;
        private static TcpClient TcpClient;

        internal static void Initialize()
        {
            TcpClient = new TcpClient() { ReceiveBufferSize = NetworkingManager.BufferSize, SendBufferSize = NetworkingManager.BufferSize };
            ReceiveBuffer = new byte[NetworkingManager.BufferSize];
        }

        public static void Connect(string IP, int Port)
        {
            TcpClient.BeginConnect(IP, Port, ConnectCallback, TcpClient);
        }

        private static void ConnectCallback(IAsyncResult _result)
        {
            TcpClient.EndConnect(_result);

            if (!TcpClient.Connected)
            {
                return;
            }

            Stream = TcpClient.GetStream();

            Stream.BeginRead(ReceiveBuffer, 0, NetworkingManager.BufferSize, ReceiveCallback, null);
        }

        private static void ReceiveCallback(IAsyncResult _result)
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

        private static void HandleData(ByteArraySource data)
        {
            DataReader dataReader = new DataReader(data);
            string PacketID = dataReader.ReadString();

            var PacketType = NetworkingManager.RegisteredPackets.Find(x => x.Item1 == PacketID).Item2;
            var Packet = (PacketBase)Activator.CreateInstance(PacketType);
            Packet.Data = data;


            Packet.OnPacketReceived(dataReader, null);
        }

        public static void SendPacket(PacketBase Packet)
        {
            try
            {
                if(TcpClient == null)
                {
                    MyLog.Default.Write("Error sending Packet to server because the Client was not initialized.", LogSeverity.Warning);
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
                MyLog.Default.Write("Error sending Packet to server", LogSeverity.Warning);
                MyLog.Default.Write(ex);
            }
        }

        public static void Disconnect()
        {
            TcpClient.Close();
            MyLog.Default.Write("TcpClient Disconnect", LogSeverity.Info);
        }
    }
}
