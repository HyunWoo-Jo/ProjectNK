using UnityEngine;

namespace N.Game
{
    public abstract class BattleLogics : MonoBehaviour
    {
        public abstract void WorkBattle();
    }

    public class StandardBattleLogic : BattleLogics {
        public override void WorkBattle() {
           
        }
    }
}
