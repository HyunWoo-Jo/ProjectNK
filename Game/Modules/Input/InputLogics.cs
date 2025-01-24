using UnityEngine;
using N.Data;
using N.UI;
using System;
using static Codice.Client.Common.Connection.AskCredentialsToUser;
namespace N.Game
{
    public enum InputLogicClassName {
        InputCombatAimLogic,
        InputBottomPortraitLogic,
        InputReloadingUILogic,
    }

    /// <summary>
    /// UI ������ Input Logic ó��
    /// </summary>
    public abstract class InputLogic : MonoBehaviour
    {
        protected InGameData _gameData;
        protected UI_Controller _uiController;
        public bool isWork;

        public void Init(InGameData gameData, UI_Controller uiController) {
            isWork = true;
            _gameData = gameData;
            _uiController = uiController;
        }

        public abstract void WorkInput();
        /// <summary>
        /// UI ����
        /// </summary>
        public abstract void Instance_UI();
    }
    /// <summary>
    /// ���� UI ���� �� �̵� ����
    /// </summary>
    public class InputCombatAimLogic : InputLogic {
        private AimView_UI _aimView;
        public override void WorkInput() {
            if (Input.GetMouseButton(0)) {
                Vector3 mouseDirection = Input.mousePositionDelta;
                // limit velocity
                float limitVelocity = 3f;
                if(mouseDirection.magnitude > limitVelocity) {
                    mouseDirection.Normalize();
                    mouseDirection *= limitVelocity;
                }

                //// Aim UI Move
                _aimView.AddPosition(mouseDirection * Time.deltaTime * Settings.CursorSpeed);
                Vector2 aimPos = _aimView.GetPosition();
                _gameData.screenPosition = _aimView.ScreenPosition();

                // Screen Limit ����
                Vector2 cameraPos;
                cameraPos.x = aimPos.x * Settings.ViewSpeed;
                cameraPos.y = aimPos.y * Settings.ViewSpeed * 0.3f;
                cameraPos.x = Mathf.Clamp(cameraPos.x, -_gameData.limitPos.x, _gameData.limitPos.x);
                cameraPos.y = Mathf.Clamp(cameraPos.y, -_gameData.limitPos.y, _gameData.limitPos.y);
                _gameData.cameraPivotTr.position = cameraPos;
            }
        }

        public override void Instance_UI() {
            // aim ui ������ ���� �ʱ�ȭ
            _aimView = _uiController.InstantiateUI<AimView_UI>();
            // aim view �ʱ�ȭ
            _aimView.SetScreenSize(new Vector2(Screen.width, Screen.height));
            var playLogic = MainLogicManager.Instance.curPlayMainLogic;
            foreach (var character in playLogic._fieldCharacter_list) {
                // �� ĳ���Ϳ� Shoot UI ���� �Ҵ�
                character.AddShootEventHandler(UpdateAimUI);
                character.AddReloadingEventHandler(UpdateReloading);
            }
            // ĳ���Ͱ� ����ɶ� AIm Ammo UI�� ����ǵ��� event �Ҵ�
            playLogic.AddChangeSlotEventHandler(UpdateAmmoChange);
            
        }
        private void UpdateAimUI(int curAmmo) {
            _aimView.SetAmmo(curAmmo);
        }
       
        private void UpdateAmmoChange(Character chracter) {
            _aimView.ReloadAmmo(chracter.GetMaxAmmo, chracter.GetAmmo);
        }
        private void UpdateReloading(Weapon weapon) {
            _aimView.ReloadAmmo(weapon.MaxAmmo, weapon.CurAmmo);
        }
       
       
    }
    public class InputReloadingUILogic : InputLogic {
        private ReloadingView_UI _reView;
        public override void WorkInput() {
           
        }
        public override void Instance_UI() {
            // reloading ui ���� �� �ʱ�ȭ
            _reView = _uiController.InstantiateUI<ReloadingView_UI>(false);
            var playLogic = MainLogicManager.Instance.curPlayMainLogic;

            foreach(var character in playLogic._fieldCharacter_list) {
                character.AddReloadingEventHandler(UpdateReloadingUI);
                // �Ѿ��� �ִ� ���¿��� ���°� ����Ǹ� UI �� 
                character.AddChangeStateEventHandler(UpdateChangeStateAimUI);
            }

        }
        /// <summary>
        /// ���� Ui ����
        /// </summary>
        /// <param name="amount"></param>
        private void UpdateReloadingUI(Weapon weapon) {
            float amount = weapon.CurloadTime / weapon.ReloadTime;
            //UI Update
            if (amount >= 1 || amount < 0) { // ���� �Ұ���
                _reView.gameObject.SetActive(false); 
            } else { // ���� ����
                _reView.gameObject.SetActive(true);
                _reView.UpdateFill(amount);
            }
        }
        private void UpdateChangeStateAimUI(CharacterState state, int curAmmo) {
            if (state == CharacterState.Standing && curAmmo > 0) {
                _reView.gameObject.SetActive(false);
            }
        }
    }


    public class InputBottomPortraitLogic : InputLogic {
        SelecteBottomPortraitView_UI sbpView;
        public override void WorkInput() {
     
        }
        public override void Instance_UI() {
            // bottom view �ʱ�ȭ // main canvas �ּ� ������ x 1440
            sbpView = _uiController.InstantiateUI<SelecteBottomPortraitView_UI>();
            sbpView.ButtonInit(_gameData.characterObj_list.Count, 1440, MainLogicManager.Instance.curPlayMainLogic.ChangeSlot);
            var playLogic = MainLogicManager.Instance.curPlayMainLogic;

            foreach (var character in playLogic._fieldCharacter_list) {
                character.AddUpdateCharacterDataHandler(UpdateBottomUI);
            }
        }

        private void UpdateBottomUI(Character character) {
            int fieldIndex = character.FieldIndex;
            sbpView.SetAmmo(fieldIndex, character.GetMaxAmmo, character.GetAmmo);
            sbpView.SetHp(fieldIndex, character.MaxHp, character.CurHp);
        }
    }
}
