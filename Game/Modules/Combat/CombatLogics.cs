using UnityEngine;
using N.Data;
namespace N.Game
{
    public enum CombatLogicClassName {
        StandardCombatLogic,

    }

    public abstract class CombatLogic : MonoBehaviour
    {
        private InGameData _gameData;
        public abstract void WorkCombat();

        public void Init(InGameData data) {
            _gameData = data;
        }
    }

    public class StandardCombatLogic : CombatLogic {
        public override void WorkCombat() {
           
        }
    }
}
