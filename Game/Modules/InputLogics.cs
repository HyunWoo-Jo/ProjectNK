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
    /// UI 생성과 Input Logic 처리
    /// </summary>
    public abstract class InputLogic : MonoBehaviour, ILogic
    {
        protected InGameData _gameData;
        protected UI_Controller _uiController;
        protected PlayMainLogic _playMainLogic;
        public bool isWork;
        protected static bool _isLockClick = false; // 클릭을 잠구는 함수
        public static bool IsLockClick {  get { return _isLockClick; } }
        public void Init(PlayMainLogic playMain, InGameData gameData, UI_Controller uiController) {
            isWork = true;
            _playMainLogic = playMain;
            _gameData = gameData;
            _uiController = uiController;
        }
        /// <summary>
        /// 매 프레임 update에서 동작
        /// </summary>
        public abstract void Work();
        /// <summary>
        /// UI 생성
        /// </summary>
        /// 
        public abstract void Instance();
        protected static void LockClick(bool isLock) {
            _isLockClick = isLock;
        }

    }
    /// <summary>
    /// 에임 UI 생성 및 이동 로직
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

                // Screen Limit 지정
                Vector2 cameraPos;
                cameraPos.x = aimPos.x * Settings.ViewSpeed;
                cameraPos.y = aimPos.y * Settings.ViewSpeed * 0.3f;
                cameraPos.x = Mathf.Clamp(cameraPos.x, -_gameData.limitPos.x, _gameData.limitPos.x);
                cameraPos.y = Mathf.Clamp(cameraPos.y, -_gameData.limitPos.y, _gameData.limitPos.y);
                _gameData.cameraPivotTr.position = cameraPos;
            }
        }

        public override void Instance() {
            // aim ui 프리팹 생성 초기화
            _aimView = _uiController.InstantiateUI<AimView_UI>(2);
            // aim view 초기화
            _aimView.SetScreenSize(new Vector2(Screen.width, Screen.height));
            foreach (var character in _playMainLogic._fieldCharacter_list) {
                // 각 캐릭터에 Shoot UI 갱신 할당
                character.AddShootEventHandler(UpdateAimUI);
                character.AddReloadingEventHandler(UpdateReloading);
            }
            // 캐릭터가 변경될때 AIm Ammo UI도 변경되도록 event 할당
            _playMainLogic.AddChangeSlotEventHandler(UpdateAmmoChange);
            
        }
        private void UpdateAimUI(Character character) {
            // 선택 유닛일 경우에만 업데이트
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
            // reloading ui 생성 및 초기화
            _reView = _uiController.InstantiateUI<ReloadingView_UI>(3,false);

            foreach(var character in _playMainLogic._fieldCharacter_list) {
                // 리로딩 상태에서 업데이트
                character.AddReloadingEventHandler(UpdateReloadingUI);
                // 총알이 있는 상태에서 상태가 변경되면 UI 끔 
                character.AddChangeStateEventHandler(UpdateChangeStateAimUI);
            }

        }
        /// <summary>
        /// 장전 Ui 갱신
        /// </summary>
        /// <param name="amount"></param>
        private void UpdateReloadingUI(Character character) {
            Weapon weapon = character.Weapon;
            float amount = weapon.CurloadTime / weapon.ReloadTime;
            //UI Update
            if (amount >= 1 || amount <= 0) { // 장전 불가능
                _reView.gameObject.SetActive(false); 
            } else { // 장전 가능
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
            // bottom view 초기화 // main canvas 최소 고정값 x 1440
            sbpView = _uiController.InstantiateUI<SelecteBottomPortraitView_UI>(1);
            // 버튼 클릭시 PlayMainLogic.ChangeSlot 호출/ 버튼에 Enter시 lock ture, eixt시 lock false
            sbpView.ButtonInit(_gameData.characterObj_list.Count, 1440, _playMainLogic.ChangeSlot, LockClick);
            // 슬롯 변경시
            _playMainLogic.AddChangeSlotEventHandler(UpdateSeleteBottomUI);

            int index = 0;
            // field에 있는 모든 캐릭터 업데이트
            foreach (var character in _playMainLogic._fieldCharacter_list) {
                // 캐릭터 데이터가 업데이트 될시
                character.AddUpdateCharacterDataHandler(UpdateBottomUI);

                // 초상화 업데이트

                string portraitName = character.Stats.portraitName;
                Sprite portrait = DataManager.Instance.LoadAssetSync<Sprite>(portraitName);
                sbpView.SetPortrait(index, portrait);
                sbpView.OnPortraitClick(index, false);
                ++index;
            }
        }
        // ammo, hp, reloding 업데이트
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

        // 선택된 버튼 UI 변경
        private void UpdateSeleteBottomUI(Character seletedCharacter) {
            int fieldIndex = seletedCharacter.FieldIndex;
            if (_seletedIndex != -1) {
                // 이전 ui port를 원래 위치로 이동
                sbpView.OnPortraitClick(_seletedIndex, false);
            }
            // 선택된 ui 위치를 변경
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
                // 추가 로직
                if (!_enemy_dic.TryGetValue(enemyInstanceID, out hpBarView)) {
                    hpBarView = _uiController.InstantiateUI<EnemyHpBarView_UI>(0);
                    hpBarView.transform.SetParent(_parentCanvasTransform.transform);
                    _enemy_dic.Add(enemyInstanceID, hpBarView);
                    // enemy의 hp가 변동될때 ui를 갱신하도록 설정
                    enemy.AddUpdateHpHandler(hpBarView.SetImageFill);       
                }
                // 위치 갱신
                hpBarView.SetPosition(enemy.transform.position);
           }
           // 없는 enemy ui 정리
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
