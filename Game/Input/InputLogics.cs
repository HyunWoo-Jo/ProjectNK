using UnityEngine;
using N.Data;

namespace N.Game
{
    public enum InputLogicClassName {
        InputLimitAimLogic,

    }

    public abstract class InputLogic : MonoBehaviour
    {
        protected InGameData _gameData;
        public bool isWork;

        public void Init(InGameData gameData) {
            isWork = true;
            _gameData = gameData;
        }

        public abstract void WorkInput();
    }

    public class InputLimitAimLogic : InputLogic {

        public override void WorkInput() {
            if (Input.GetMouseButton(0)) {
                Vector3 mouseDirection = Input.mousePositionDelta;

                Vector2 newPos = _gameData.cameraPivotTr.position + (mouseDirection * Settings.CursorSpeed * Time.deltaTime);
                newPos.x = Mathf.Clamp(newPos.x, -_gameData.limitPos.x, _gameData.limitPos.x);
                newPos.y = Mathf.Clamp(newPos.y, -_gameData.limitPos.y, _gameData.limitPos.y);
                _gameData.cameraPivotTr.position = newPos;
            }
        }
    }

}
