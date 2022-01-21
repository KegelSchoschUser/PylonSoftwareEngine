using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PylonGameEngine.Utilities
{
    public class DiscordRPC
    {
        private DiscordRpcClient DiscordRpcClient;
        private RichPresence RichPresence;

        public DiscordRPC(string ApplicationID, string GameName)
        {
            DiscordRpcClient = new DiscordRpcClient(ApplicationID);

            DiscordRpcClient.Initialize();

            RichPresence = new RichPresence();
            RichPresence.Details = "Playing " + GameName;
            RichPresence.Assets = new Assets() {LargeImageKey = "logo", LargeImageText = "Round 1"};

            RichPresence.State = "Playing Solo ;(";
            
            DiscordRpcClient.SetPresence(RichPresence);
        }

        public void Update()
        {
            DiscordRpcClient.Invoke();
        }
    }
}
