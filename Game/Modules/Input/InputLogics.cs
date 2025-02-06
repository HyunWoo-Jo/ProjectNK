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
        protected static bool _isLockClick = false; // Ŭ���� �ᱸ�� �Լ�
        public static bool IsLockClick {  get { return _isLockClick; } }
        public void Init(InGameData gameData, UI_Controller uiController) {
            isWork = true;
            _gameData = gameData;
            _uiController = uiController;
        }
        /// <summary>
        /// �� ������ update���� ����
        /// </summary>
        public abstract void WorkInput();
        /// <summary>
        /// UI ����
        /// </summary>
        public abstract void Instance_UI();

        protected static void LockClick(bool isLock) {
            _isLockClick = isLock;
        }
    }
    /// <summary>
    /// ���� UI ���� �� �̵� ����
    /// </summary>
    public class InputCombatAimLogic : InputLogic {
        private AimView_UI _aimView;
        public override void WorkInput() {
            if (!isWork) return;

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
        private void UpdateAimUI(Character character) {
            _aimView.SetAmmo(character.GetAmmo);
        }
       
        private void UpdateAmmoChange(Character chracter) {
            _aimView.ReloadAmmo(chracter.GetMaxAmmo, chracter.GetAmmo);
        }
        private void UpdateReloading(Character chracter) {
            Weapon weapon = chracter.Weapon;
            _aimView.ReloadAmmo(weapon.MaxAmmo, weapon.CurAmmo);
        }
       
       
    }
    public class InputReloadingUILogic : InputLogic {
        private ReloadingView_UI _reView;
        public override void WorkInput() {
            if (!isWork) return;
        }
        public override void Instance_UI() {
            // reloading ui ���� �� �ʱ�ȭ
            _reView = _uiController.InstantiateUI<ReloadingView_UI>(false);
            var playLogic = MainLogicManager.Instance.curPlayMainLogic;

            foreach(var character in playLogic._fieldCharacter_list) {
                // ���ε� ���¿��� ������Ʈ
                character.AddReloadingEventHandler(UpdateReloadingUI);
                // �Ѿ��� �ִ� ���¿��� ���°� ����Ǹ� UI �� 
                character.AddChangeStateEventHandler(UpdateChangeStateAimUI);
            }

        }
        /// <summary>
        /// ���� Ui ����
        /// </summary>
        /// <param name="amount"></param>
        private void UpdateReloadingUI(Character character) {
            Weapon weapon = character.Weapon;
            float amount = weapon.CurloadTime / weapon.ReloadTime;
            //UI Update
            if (amount >= 1 || amount <= 0) { // ���� �Ұ���
                _reView.gameObject.SetActive(false); 
            } else { // ���� ����
                _reView.gameObject.SetActive(true);
                _reView.UpdateFill(amount);
            }
        }
        private void UpdateChangeStateAimUI(Character character) {
            if (character.State == CharacterState.Standing && character.GetAmmo > 0) {
                _reView.gameObject.SetActive(false);
            }
        }
    }


    public class InputBottomPortraitLogic : InputLogic {
        SelecteBottomPortraitView_UI sbpView;
        private int _seletedIndex = -1;
        public override void WorkInput() {
            if (!isWork) return;
        }
        public override void Instance_UI() {
            // bottom view �ʱ�ȭ // main canvas �ּ� ������ x 1440
            sbpView = _uiController.InstantiateUI<SelecteBottomPortraitView_UI>();
            // ��ư Ŭ���� PlayMainLogic.ChangeSlot ȣ��/ ��ư�� Enter�� lock ture, eixt�� lock false
            sbpView.ButtonInit(_gameData.characterObj_list.Count, 1440, MainLogicManager.Instance.curPlayMainLogic.ChangeSlot, LockClick);
            var playLogic = MainLogicManager.Instance.curPlayMainLogic;
            // ���� �����
            playLogic.AddChangeSlotEventHandler(UpdateSeleteBottomUI);

            int index = 0;
            // field�� �ִ� ��� ĳ���� ������Ʈ
            foreach (var character in playLogic._fieldCharacter_list) {
                // ĳ���� �����Ͱ� ������Ʈ �ɽ�
                character.AddUpdateCharacterDataHandler(UpdateBottomUI);

                // �ʻ�ȭ ������Ʈ

                string portraitName = character.Stats.portraitName;
                Sprite portrait = DataManager.Instance.LoadAssetSync<Sprite>(portraitName);
                sbpView.SetPortrait(index, portrait);
                sbpView.OnPortraitClick(index, false);
                ++index;
            }
        }
        // ammo, hp, reloding ������Ʈ
        private void UpdateBottomUI(Character character) {
            int fieldIndex = character.FieldIndex;
            sbpView.SetAmmo(fieldIndex, character.GetMaxAmmo, character.GetAmmo);
            sbpView.SetHp(fieldIndex, character.MaxHp, character.CurHp);

            Weapon weapon = character.Weapon;
            float amount = weapon.CurloadTime / weapon.ReloadTime;
            if (amount > 0 && amount < 1) {
                sbpView.SetReloadingActive(character.FieldIndex, true);
                sbpView.SetReloading(character.FieldIndex, amount);
            } else {
                sbpView.SetReloadingActive(character.FieldIndex, false);
            }
        }

        // ���õ� ��ư UI ����
        private void UpdateSeleteBottomUI(Character seletedCharacter) {
            int fieldIndex = seletedCharacter.FieldIndex;
            if (_seletedIndex != -1) {
                // ���� ui port�� ���� ��ġ�� �̵�
                sbpView.OnPortraitClick(_seletedIndex, false); 
            }
            // ���õ� ui ��ġ�� ����
            sbpView.OnPortraitClick(fieldIndex, true);
            _seletedIndex = fieldIndex;
        }
    }
}
