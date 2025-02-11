using UnityEngine;
using N.Data;
using N.DesignPattern;
using System;
using N.Utills;
using Unity.Android.Gradle.Manifest;
namespace N.Game
{
    public enum CharacterState {
        Standing,
        Sitting,
        AI,
        Hide,
    }

    public enum CharacterWeaponCreateSetting { // 캐릭터 무기를 어떤 방식으로 생성할지 정의 하는 셋팅
        Standard, // 캐릭터 기본 무기로 생성
        Null, // 생성하지 않음 (직접 생성 후 넣어줘야함)
    }

    public class Character : MonoBehaviour
    {
        [ReadOnly] [SerializeField] private CharacterState _state;
        public CharacterState State { get { return _state; } }
        [ReadOnly] [SerializeField] private CharacterStats _stats;
        public CharacterStats Stats { get { return _stats; } }
        public float CurHp => _stats.curHp;
        public float MaxHp => _stats.hp;
        public Vector3 targetPos;
        private CharacterAI _ai;
        private Weapon _weapon;

        public Weapon Weapon { get { return _weapon; } }

        private GameObject _model;
        private Sprite _portrait;
        private InGameData _gameData;

        private int _fieldIndex;
        public int FieldIndex => _fieldIndex;

        private Action<Character> _reloadingAction;
        private Action<Character> _shootAction;
        private Action<Character> _changeStateAction;
        private Action<Character> _updateCharacterAction;

        internal void Init(InGameData gameData, GameObject model, int fieldIndex, CharacterStats stats, CharacterWeaponCreateSetting settings) {
            _gameData = gameData;
            _model = model;
            SetStats(stats);
            ChangeState(CharacterState.AI);
            _fieldIndex = fieldIndex;
            switch (settings) {
                case CharacterWeaponCreateSetting.Standard:
                CreateWeapon();
                _ai = new CharacterStandardAI(this, MainLogicManager.Instance.curPlayMainLogic);
                break;
                case CharacterWeaponCreateSetting.Null:

                break;
            }      
        }
        

        internal void SetStats(CharacterStats stats) {
            _stats = stats;
        }
        internal void CreateWeapon(Weapon weapon = null) {
            if (weapon == null) {
                _weapon = new Weapon();
                GameObject bulletObj = DataManager.Instance.LoadAssetSync<GameObject>(_stats.bulletName);
                _weapon.InitWeapon(bulletObj, _stats.ammoCapacity, _stats.ammoCapacity, _stats.reloadTime, _stats.attackSpeed);
            } else {
                _weapon = weapon;
            }
        }
        internal void CreateAI<AI>() where AI : CharacterAI {
            _ai = (AI)Activator.CreateInstance(typeof(AI), this, MainLogicManager.Instance.curPlayMainLogic);
        }

        internal void Work() {
            if (_weapon == null) { return; }
            if (_state.Equals(CharacterState.Standing)) {
                if (_weapon.CurAmmo > 0) {
                    Vector2 screenPosition = _gameData.screenPosition;
                    Ray ray = Camera.main.ScreenPointToRay(screenPosition);
                    if (Physics.Raycast(ray, out var hit, 1000f, 1 << 6)) {
                        if (Shoot(hit.point)) ShootEvent();

                    }
                } else { // 총알이 없으면 자동 장전
                    Reload();
                    ReloadingEvent();
                }
            } else if (_state.Equals(CharacterState.Sitting) || _state.Equals(CharacterState.Hide)) {
                Reload();
                ReloadingEvent();
            } else if (_state.Equals(CharacterState.AI)) { 
                _ai?.Work();
            }
            if(_gameData.currentCharacterIndex == FieldIndex) {
                ReloadingEvent();
            }
            _updateCharacterAction?.Invoke(this);
        }
        internal void ChangeState(CharacterState state) {
            if (_state == state) return;
            _state = state;
            if (_weapon?.CurAmmo != 0) {
                // 특정 상태면 reloadtime 초기화를 하지않음
                if (!state.Equals(CharacterState.AI) || !state.Equals(CharacterState.Sitting)) {
                    _weapon?.ResetReloadTime();
                }
                // event 발생 ui 업데이트 관련
                _changeStateAction?.Invoke(this);
            }
        }
        internal int GetAmmo => _weapon.CurAmmo;
        internal int GetMaxAmmo => _weapon.MaxAmmo;

        internal bool Shoot(Vector3 targetPos) {
            return _weapon.Shot(_model.transform.position, targetPos, _stats.attack);
        }
        internal void ShootEvent() {
            _shootAction?.Invoke(this);
        }

        /// <summary>
        /// 장전
        /// </summary>
        internal void Reload() {
            _weapon.Reloading();
        }
        /// <summary>
        /// 장전 UI 업데이트
        /// </summary>
        internal void ReloadingEvent() {
            // 장전 event 
            _reloadingAction?.Invoke(this);
        }
        public void AddReloadingEventHandler(Action<Character> action) {
            _reloadingAction += action;
        }

        public void AddShootEventHandler(Action<Character> action) {
            _shootAction += action;
        }
        public void AddChangeStateEventHandler(Action<Character> action) {
            _changeStateAction += action;
        }
        public void AddUpdateCharacterDataHandler(Action<Character> action) {
            _updateCharacterAction += action;
        }
    }
}
