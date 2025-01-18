using N.DesignPattern;
using UnityEngine;

namespace N.Game
{
    public class Weapon
    {
        private ObjectPool<Bullet> _bulletPool;
        private int _maxAmmo;
        private int _curAmmo;
        private float _reloadTime;
        private float _curloadTime;
        private float _rpm;
        private float _attackTime;

        internal int MaxAmmo { get { return _maxAmmo; } }
        internal int CurAmmo { get { return _curAmmo; } }

        internal float ReloadTime { get { return _reloadTime; } }
        internal float CurloadTime {  get { return _curloadTime; } }

        internal void InitWeapon(GameObject bulletObj, int ammo, int curAmmo, float reloadTime, float attackSpeed) {
            if(_bulletPool != null) {
                _bulletPool.Dipose();
                _bulletPool = null;
            }
            _bulletPool = ObjectPool<Bullet>.Instance(bulletObj, 20);
            _maxAmmo = ammo;
            _curAmmo = curAmmo;
            _reloadTime = reloadTime;
            _rpm = 1 / attackSpeed ; // atack speed -> rpm
        }

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
        /// <returns> 장전 불가능 -1 | 가능 _curloadTime / _reloadTime </returns>
        internal float Reloading() {
            if (_curAmmo >= _maxAmmo) return -1f;
            _curloadTime += Time.deltaTime;
            if(_reloadTime < _curloadTime) {
                Reload();
            }
            return _curloadTime / _reloadTime;
        }
        /// <summary>
        /// 즉시 장전
        /// </summary>
        internal void Reload() {
            _curAmmo = _maxAmmo;
        }
        /// <summary>
        /// 총 발사
        /// </summary>
        internal bool Shot(Vector3 startPos, Vector3 targetPos, float damage ) {
            if (_curAmmo > 0) {
                _attackTime += Time.deltaTime;
                if (_attackTime >= _rpm) {
                    _attackTime -= _rpm;
                    
                    Bullet bullet = _bulletPool.BorrowItem();
                    bullet.SetOwner(Owner.Player);
                    bullet.transform.position = startPos;
                    bullet.SetTarget(targetPos, damage);
                    
                    bullet.gameObject.SetActive(true);
                    --_curAmmo;
                    return true;
                }
            }
            return false;
        }
    }



}
