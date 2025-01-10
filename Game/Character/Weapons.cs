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
        /// ������ ����
        /// </summary>
        /// <returns></returns>
        internal float ReloadRation() {
            return Mathf.Lerp(0, 1, _curloadTime / _reloadTime);
        }
        /// <summary>
        /// ���� Ÿ�̸� �ʱ�ȭ
        /// </summary>
        internal void ResetReloadTime() {
            _curloadTime = 0;
        }

        /// <summary>
        /// ������ ���� �ð��� �Ǹ� ����
        /// </summary>
        internal void Reloading() {
            _curloadTime += Time.deltaTime;
            if(_reloadTime < _curloadTime) {
                Reload();
            }
        }
        /// <summary>
        /// ��� ����
        /// </summary>
        internal void Reload() {
            _curAmmo = _ammo;
        }
        /// <summary>
        /// �� �߻�
        /// </summary>
        internal void Shot() {

        }
    }



}
