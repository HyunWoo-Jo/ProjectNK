using UnityEngine;

namespace N.Game
{
    public abstract class CombatLogic : MonoBehaviour
    {
        public abstract void WorkCombat();
    }

    public class StandardCombatLogic : CombatLogic {
        public override void WorkCombat() {
           
        }
    }
}
