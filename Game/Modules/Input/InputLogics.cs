using UnityEngine;
using N.Data;

namespace N.Game
{
    public enum InputLogicClassName {
        InputScreenLimitLogic,
        InputCombatAimLogic,
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
        /// <summary>
        /// UI »ý¼º
        /// </summary>
        public virtual void Instance_UI() { }
    }

    public class InputScreenLimitLogic : InputLogic {

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

    public class InputCombatAimLogic : InputLogic {
        public override void WorkInput() {

        }

        public override void Instance_UI() {
            base.Instance_UI();
            GameObject prefab = DataManager.Instance.LoadAssetSync<GameObject>(Settings.aimPrefabName);
            GameObject obj = GameObject.Instantiate(prefab);
            obj.transform.SetParent(_gameData.mainCanvas.transform);
            obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
        }
    }
}
