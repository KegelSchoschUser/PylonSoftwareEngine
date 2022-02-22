using PylonGameEngine.General;

namespace PylonGameEngine.GameWorld
{
    public class GameScript : Component3D
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
