using DiscordRPC;

namespace PylonSoftwareEngine.Utilities
{
    public class DiscordRPC
    {
        private DiscordRpcClient DiscordRpcClient;
        private RichPresence RichPresence;

        public DiscordRPC(string ApplicationID, string SoftwareName)
        {
            DiscordRpcClient = new DiscordRpcClient(ApplicationID);

            DiscordRpcClient.Initialize();

            RichPresence = new RichPresence();
            RichPresence.Details = "Playing " + SoftwareName;
            RichPresence.Assets = new Assets() { LargeImageKey = "logo", LargeImageText = "Round 1" };

            RichPresence.State = "Playing Solo ;(";

            DiscordRpcClient.SetPresence(RichPresence);
        }

        public void Update()
        {
            DiscordRpcClient.Invoke();
        }
    }
}
