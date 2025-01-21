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

    public enum CharacterWeaponCreateSetting { // 캐릭터 무기를 어떤 방식으로 생성할지 정의 하는 셋팅
        Standard, // 캐릭터 기본 무기로 생성
        Null, // 생성하지 않음 (직접 생성 후 넣어줘야함)
    }

    public class Character : MonoBehaviour
    {
        private CharacterState _state;
        private CharacterStats _stats;
        private CharacterAI _ai;
        private Weapon _weapon;

        private GameObject _model;
        private Sprite _portrait;
        private InGameData _gameData;
        internal void Init(InGameData gameData, GameObject model, CharacterStats stats, CharacterWeaponCreateSetting settings) {
            _gameData = gameData;
            _model = model;
            SetStats(stats);
            ChangeState(CharacterState.AI);
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
            if (_state.Equals(CharacterState.Standing)) {
                if (_weapon.CurAmmo > 0) {
                    Vector2 screenPosition = _gameData.aimView.ScreenPosition();
                    Ray ray = Camera.main.ScreenPointToRay(screenPosition);
                    if (Physics.Raycast(ray, out var hit, 1000f, 1 << 6)) {
                        if (_weapon.Shot(_model.transform.position, hit.point, _stats.attack)) {
                            // UI 업데이트
                            _gameData.aimView.SetAmmo(_weapon.CurAmmo);
                        }
                    }
                } else {
                    UpdateReloading();
                }
            } else if (_state.Equals(CharacterState.Sitting) || _state.Equals(CharacterState.Hide)) {
                UpdateReloading();
            } else if (_state.Equals(CharacterState.AI)) {
                _ai?.Work();
            }
        }
        internal void ChangeState(CharacterState state) {
            _state = state;
            if (_weapon?.CurAmmo != 0) {
                _weapon?.ResetReloadTime();
                _gameData.reloadingUI?.gameObject.SetActive(false);
            }
        }
        internal int GetAmmo => _weapon.CurAmmo;
        internal int GetMaxAmmo => _weapon.MaxAmmo;

        /// <summary>
        /// 장전과 UI 업데이트
        /// </summary>
        private void UpdateReloading() {
            float amount = _weapon.Reloading();
            //UI Update
            if (amount >= 1) {
                _gameData.reloadingUI.gameObject.SetActive(false);
                _gameData.aimView.SetAmmo(_weapon.CurAmmo);
            } else if (amount < 0) {
                _gameData.reloadingUI.gameObject.SetActive(false);
            } else {
                _gameData.reloadingUI.gameObject.SetActive(true);
                _gameData.reloadingUI.UpdateFill(amount);
            }
        }
    }
}
