using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PylonGameEngine.Physics
{
    internal enum CollisionType
    {
        None,
        Rigid,
        Static,
        Trigger
    }

    internal static class CollisionCheck
    {
        private static Dictionary<CollisionType, bool> CollisionEnabled = new Dictionary<CollisionType, bool>()
        {
            {CollisionType.Rigid, true},
            {CollisionType.Static, true},
            {CollisionType.Trigger, false},
        };

        public static bool CanCollideByType(CollisionType A, CollisionType B)
        {
            bool A_CanCollide = CollisionEnabled[A];
            bool B_CanCollide = CollisionEnabled[B];

            if(A_CanCollide && B_CanCollide)
            {
                return true;
            }

            return false;
        }

        public static bool CanCollideByObject(bool A, bool B)
        {
            if (A && B)
                return true;

            return false;
        }

        public static bool CanCollide(CollisionType A_Type, CollisionType B_Type, bool A_Collision, bool B_Collision)
        {
            if (CanCollideByType(A_Type, B_Type) && CanCollideByObject(A_Collision, B_Collision))
            {
                return true;
            }

            return false;
        }

        public static bool ObjectCollidedTrigger(CollisionType A_Type, CollisionType B_Type)
        {
            bool A_Type_Collision = CollisionEnabled[A_Type];
            bool B_Type_Collision = CollisionEnabled[A_Type];

            bool AB = A_Type_Collision && (B_Type == CollisionType.Trigger);
            bool BA = B_Type_Collision && (A_Type == CollisionType.Trigger);

            if (AB || BA)
            {
                return true;
            }


            return false;
        }
    }
}
