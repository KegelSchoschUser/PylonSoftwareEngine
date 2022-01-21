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
            RichPresence.Timestamps = new Timestamps() { Start = DateTime.Now, End = DateTime.Now.AddMinutes(15)};

            RichPresence.Party = new Party() { ID = "0", Size = -1, Max = 10, Privacy = Party.PrivacySetting.Public};
            RichPresence.Secrets = new Secrets() { JoinSecret = "0", MatchSecret = Secrets.CreateFriendlySecret(new Random()) , SpectateSecret = "0"};
            
            DiscordRpcClient.RegisterUriScheme(executable: "editor.exe");
            DiscordRpcClient.SetPresence(RichPresence);
        }

        public void Update()
        {
            DiscordRpcClient.Invoke();
        }
    }
}
