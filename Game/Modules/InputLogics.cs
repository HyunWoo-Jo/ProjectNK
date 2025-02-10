using UnityEngine;
using N.Data;
using N.UI;
using System;
using static Codice.Client.Common.Connection.AskCredentialsToUser;
using N.DesignPattern;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;
namespace N.Game
{
    public enum InputLogicClassName {
        InputCombatAimLogic,
        InputBottomPortraitLogic,
        InputReloadingUILogic,
        InputEnemyHpBarLogic,
    }


    /// <summary>
    /// UI ������ Input Logic ó��
    /// </summary>
    public abstract class InputLogic : MonoBehaviour, ILogic
    {
        protected InGameData _gameData;
        protected UI_Controller _uiController;
        protected PlayMainLogic _playMainLogic;
        public bool isWork;
        protected static bool _isLockClick = false; // Ŭ���� �ᱸ�� �Լ�
        public static bool IsLockClick {  get { return _isLockClick; } }
        public void Init(PlayMainLogic playMain, InGameData gameData, UI_Controller uiController) {
            isWork = true;
            _playMainLogic = playMain;
            _gameData = gameData;
            _uiController = uiController;
        }
        /// <summary>
        /// �� ������ update���� ����
        /// </summary>
        public abstract void Work();
        /// <summary>
        /// UI ����
        /// </summary>
        /// 
        public abstract void Instance();
        protected static void LockClick(bool isLock) {
            _isLockClick = isLock;
        }

    }
    /// <summary>
    /// ���� UI ���� �� �̵� ����
    /// </summary>
    public class InputCombatAimLogic : InputLogic {
        private AimView_UI _aimView;
        private int _seletedIndex = -1;
        public override void Work() {
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

        public override void Instance() {
            // aim ui ������ ���� �ʱ�ȭ
            _aimView = _uiController.InstantiateUI<AimView_UI>(2);
            // aim view �ʱ�ȭ
            _aimView.SetScreenSize(new Vector2(Screen.width, Screen.height));
            foreach (var character in _playMainLogic._fieldCharacter_list) {
                // �� ĳ���Ϳ� Shoot UI ���� �Ҵ�
                character.AddShootEventHandler(UpdateAimUI);
                character.AddReloadingEventHandler(UpdateReloading);
            }
            // ĳ���Ͱ� ����ɶ� AIm Ammo UI�� ����ǵ��� event �Ҵ�
            _playMainLogic.AddChangeSlotEventHandler(UpdateAmmoChange);
            
        }
        private void UpdateAimUI(Character character) {
            // ���� ������ ��쿡�� ������Ʈ
            if (_seletedIndex == character.FieldIndex) {
                _aimView.SetAmmo(character.GetAmmo);
            }
        }
       
        private void UpdateAmmoChange(Character character) {
            _seletedIndex = character.FieldIndex;
            _aimView.ReloadAmmo(character.GetMaxAmmo, character.GetAmmo);
        }
        private void UpdateReloading(Character character) {
            if (_seletedIndex == character.FieldIndex) {
                Weapon weapon = character.Weapon;
                _aimView.ReloadAmmo(weapon.MaxAmmo, weapon.CurAmmo);
            }
        }
       
       
    }
    public class InputReloadingUILogic : InputLogic {
        private ReloadingView_UI _reView;
        public override void Work() {
            if (!isWork) return;
        }
        public override void Instance() {
            // reloading ui ���� �� �ʱ�ȭ
            _reView = _uiController.InstantiateUI<ReloadingView_UI>(3,false);

            foreach(var character in _playMainLogic._fieldCharacter_list) {
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
        public override void Work() {
            if (!isWork) return;
        }
        public override void Instance() {
            // bottom view �ʱ�ȭ // main canvas �ּ� ������ x 1440
            sbpView = _uiController.InstantiateUI<SelecteBottomPortraitView_UI>(1);
            // ��ư Ŭ���� PlayMainLogic.ChangeSlot ȣ��/ ��ư�� Enter�� lock ture, eixt�� lock false
            sbpView.ButtonInit(_gameData.characterObj_list.Count, 1440, _playMainLogic.ChangeSlot, LockClick);
            // ���� �����
            _playMainLogic.AddChangeSlotEventHandler(UpdateSeleteBottomUI);

            int index = 0;
            // field�� �ִ� ��� ĳ���� ������Ʈ
            foreach (var character in _playMainLogic._fieldCharacter_list) {
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

    // Enemy HP UI
    public class InputEnemyHpBarLogic : InputLogic {
        private Dictionary<int, EnemyHpBarView_UI> _enemy_dic = new();
        private GameObject _parentCanvasTransform;
        public override void Instance() {
            _parentCanvasTransform = _uiController.InstantiateParentCanvas(0);
        }

        public override void Work() {
           HashSet<int> curEnemyKey_map = new(_playMainLogic._fieldEnemy_list.Count);
           foreach(var enemy in _playMainLogic._fieldEnemy_list) {
                int enemyInstanceID = enemy.GetInstanceID();
                curEnemyKey_map.Add(enemyInstanceID);
                EnemyHpBarView_UI hpBarView;
                // �߰� ����
                if (!_enemy_dic.TryGetValue(enemyInstanceID, out hpBarView)) {
                    hpBarView = _uiController.InstantiateUI<EnemyHpBarView_UI>(0);
                    hpBarView.transform.SetParent(_parentCanvasTransform.transform);
                    _enemy_dic.Add(enemyInstanceID, hpBarView);
                    // enemy�� hp�� �����ɶ� ui�� �����ϵ��� ����
                    enemy.AddUpdateHpHandler(hpBarView.SetImageFill);       
                }
                // ��ġ ����
                hpBarView.SetPosition(enemy.transform.position);
           }
           // ���� enemy ui ����
           foreach(var keyValue in _enemy_dic) {
                if (!curEnemyKey_map.Contains(keyValue.Key)) {
                    EnemyHpBarView_UI view = _enemy_dic[keyValue.Key];
                    _enemy_dic.Remove(keyValue.Key);
                    Destroy(view.gameObject);
                }
            }
        }

    }
}
