using UnityEngine;

namespace N.Game
{
    public class Weapon
    {
        private int _ammo;
        private int _curAmmo;
        private float _reloadTime;
        private float _curloadTime;
        private float _rpm;
        
        /// <summary>
        /// 재장전 비율
        /// </summary>
        /// <returns></returns>
        internal float ReloadRation() {
            return Mathf.Lerp(0, 1, _curloadTime / _reloadTime);
        }
        /// <summary>
        /// 장전 타이머 초기화
        /// </summary>
        internal void ResetReloadTime() {
            _curloadTime = 0;
        }

        /// <summary>
        /// 설정한 장전 시간이 되면 장전
        /// </summary>
        internal void Reloading() {
            _curloadTime += Time.deltaTime;
            if(_reloadTime < _curloadTime) {
                Reload();
            }
        }
        /// <summary>
        /// 즉시 장전
        /// </summary>
        internal void Reload() {
            _curAmmo = _ammo;
        }
        /// <summary>
        /// 총 발사
        /// </summary>
        internal void Shot() {

        }
    }



}
