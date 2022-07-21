using PylonGameEngine.SceneManagement;
using PylonGameEngine.Utilities;

namespace PylonGameEngine.General
{
    public class IComponent : UniqueNameInterface
    {
        public Scene SceneContext { get; internal set; }
        public float DeltaTime => MyGame.RenderLoop.DeltaTime;
        public float FixedDeltaTime => MyGame.GameTickLoop.DeltaTime;

        public virtual void Initialize()
        {

        }

        public virtual void OnDestroy()
        {

        }


        public virtual void UpdateTick()
        {

        }

        public virtual void UpdateFrame()
        {

        }
    }
}
