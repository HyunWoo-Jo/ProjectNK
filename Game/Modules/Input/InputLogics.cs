using UnityEngine;
using N.Data;
using N.UI;
using System;
namespace N.Game
{
    public enum InputLogicClassName {
        InputCombatAimLogic,
        InputBottomPortraitLogic,
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
        /// UI 생성
        /// </summary>
        public abstract void Instance_UI();
    }
    /// <summary>
    /// 에임 UI 생성 및 이동 로직
    /// </summary>
    public class InputCombatAimLogic : InputLogic {
        public override void WorkInput() {
            if (Input.GetMouseButton(0)) {
                Vector3 mouseDirection = Input.mousePositionDelta;
                // limit velocity
                float limitVelocity = 3f;
                if(mouseDirection.magnitude > limitVelocity) {
                    mouseDirection.Normalize();
                    mouseDirection *= limitVelocity;
                }

                // Aim UI Move
                _gameData.aimView.AddPosition(mouseDirection * Time.deltaTime * Settings.CursorSpeed);
                Vector2 aimPos = _gameData.aimView.GetPosition();

                // Screen Limit 지정
                Vector2 cameraPos;
                cameraPos.x = aimPos.x * Settings.ViewSpeed;
                cameraPos.y = aimPos.y * Settings.ViewSpeed * 0.3f;
                cameraPos.x = Mathf.Clamp(cameraPos.x, -_gameData.limitPos.x, _gameData.limitPos.x);
                cameraPos.y = Mathf.Clamp(cameraPos.y, -_gameData.limitPos.y, _gameData.limitPos.y);
                _gameData.cameraPivotTr.position = cameraPos;
            }
        }

        public override void Instance_UI() {
            // aim ui 프리팹 생성 초기화
            GameObject prefab = DataManager.Instance.LoadAssetSync<GameObject>(Settings.aimPrefabName);
            GameObject obj = GameObject.Instantiate(prefab);
            obj.transform.SetParent(_gameData.mainCanvas.transform);
            obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            // aim view 초기화
            _gameData.aimView = obj.GetComponent<AimView_UI>();
            _gameData.aimView.SetScreenSize(new Vector2(Screen.width, Screen.height));
        }
    }

    public class InputBottomPortraitLogic : InputLogic {
        public override void WorkInput() {
     
        }
        public override void Instance_UI() {
            // 하단 초상화 버튼 UI 생성 및 초기화
            GameObject prefab = DataManager.Instance.LoadAssetSync<GameObject>("SelectBottomPortrait_UI.prefab");
            GameObject obj = GameObject.Instantiate(prefab);
            obj.transform.SetParent(_gameData.mainCanvas.transform);
            obj.GetComponent<RectTransform>().localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            // bottom view 초기화 // main canvas 최소 고정값 x 1440
            _gameData.selecteBottomPortraitView = obj.GetComponent<SelecteBottomPortraitView_UI>();
            _gameData.selecteBottomPortraitView.ButtonInit(_gameData.characterObj_list.Count, 1440, MainLogicManager.Instance.curPlayMainLogic.ChangeSlot);
        }
    }
}
