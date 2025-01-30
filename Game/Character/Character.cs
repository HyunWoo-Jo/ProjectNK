using UnityEngine;
using N.Data;
using N.DesignPattern;
using System;
namespace N.Game
{
    public enum CharacterState {
        Standing,
        Sitting,
        AI,
        Hide,
    }

    public enum CharacterWeaponCreateSetting { // ĳ���� ���⸦ � ������� �������� ���� �ϴ� ����
        Standard, // ĳ���� �⺻ ����� ����
        Null, // �������� ���� (���� ���� �� �־������)
    }

    public class Character : MonoBehaviour
    {
        private CharacterState _state;
        public CharacterState State { get { return _state; } }
        private CharacterStats _stats;
        public float CurHp => _stats.curHp;
        public float MaxHp => _stats.hp;
        
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
        internal void Work() {
            if (_weapon == null) { return; }
            if (_state.Equals(CharacterState.Standing)) {
                if (_weapon.CurAmmo > 0) {
                    Vector2 screenPosition = _gameData.screenPosition;
                    Ray ray = Camera.main.ScreenPointToRay(screenPosition);
                    if (Physics.Raycast(ray, out var hit, 1000f, 1 << 6)) {
                        if (_weapon.Shot(_model.transform.position, hit.point, _stats.attack)) { 
                            // �߻� ���� �� UI ������Ʈ
                            _shootAction?.Invoke(this);
                        }
                    }
                } else { // �Ѿ��� ������ �ڵ� ����
                    UpdateReloading();
                }
            } else if (_state.Equals(CharacterState.Sitting) || _state.Equals(CharacterState.Hide)) {    
                UpdateReloading();
            } else if (_state.Equals(CharacterState.AI)) {
                _ai?.Work();
            }
            _updateCharacterAction?.Invoke(this);
        }
        internal void ChangeState(CharacterState state) {
            if (_state == state) return;
            _state = state;
            if (_weapon?.CurAmmo != 0) {
                _weapon?.ResetReloadTime();
                _changeStateAction?.Invoke(this);
            }
        }
        internal int GetAmmo => _weapon.CurAmmo;
        internal int GetMaxAmmo => _weapon.MaxAmmo;

        /// <summary>
        /// ������ UI ������Ʈ
        /// </summary>
        private void UpdateReloading() {
            _weapon.Reloading();
            // ���� event 
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
