using PylonGameEngine.General;

namespace PylonGameEngine.GameWorld
{
    public class GameScript : IComponent
    {
        public GameScript()
        {
            WorldManager.Scripts.Add(this);
        }

        ~GameScript()
        {
            WorldManager.Scripts.Remove(this);
        }
    }
}
