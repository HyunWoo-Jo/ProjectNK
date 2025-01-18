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
    public class Character : MonoBehaviour
    {
        private CharacterState _state;
        private CharacterStats _stats;
        private CharacterAI _ai;
        private Weapon _weapon;

        private GameObject _model;
        private Sprite _portrait;
        private InGameData _gameData;
        internal void Init(InGameData gameData, GameObject model) {
            _gameData = gameData;
            _model = model;
        }

        internal void SetStats(CharacterStats stats) {
            _stats = stats;
            ChangeState(CharacterState.AI);
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
                
                Vector2 screenPosition = _gameData.aimView.ScreenPosition();
                Ray ray = Camera.main.ScreenPointToRay(screenPosition);
                if (Physics.Raycast(ray, out var hit, 1000f, 1 << 6)) {
                    if (_weapon.Shot(_model.transform.position, hit.point, _stats.attack)) {
                        // UI 업데이트
                        _gameData.aimView.SetAmmo(_weapon.CurAmmo);
                    }
                }
            } else if (_state.Equals(CharacterState.Sitting) || _state.Equals(CharacterState.Hide)) {
               float amount = _weapon.Reloading();   
               if(amount >= 1) {
                    _gameData.reloadingUI.gameObject.SetActive(false);
                    _gameData.aimView.SetAmmo(_weapon.CurAmmo);
                }else if(amount < 0) {
                    _gameData.reloadingUI.gameObject.SetActive(false);
                } else {
                    _gameData.reloadingUI.gameObject.SetActive(true);
                    _gameData.reloadingUI.UpdateFill(amount);
                }
            } else if (_state.Equals(CharacterState.AI)) {
                //_ai.Work();
            }
        }
        internal void ChangeState(CharacterState state) {
            _state = state;
            _weapon?.ResetReloadTime();
        }
        internal int GetAmmo => _weapon.CurAmmo;
        internal int GetMaxAmmo => _weapon.MaxAmmo;
    }
}
