
using UnityEngine;
using N.Data;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.Plastic.Newtonsoft.Json.Serialization;
using NUnit.Framework;
namespace N.Game
{
    public class Enemy : MonoBehaviour, IHitAble {

        [SerializeField] private HitArea[] _hitAreas;
        private CharacterStats _stats;
        public CharacterStats Stats { get { return _stats; } }
        private Weapon _weapon;

        private int _hitIndex = 0;

        private Action<float, float> _updateHpAction;
        private void Awake() {
            _hitAreas = GetComponentsInChildren<HitArea>();
#if UNITY_EDITOR
            Assert.IsTrue(_hitAreas.Length > 0, "Enemy" + gameObject.name);
#endif
            Init();
        }
        public void Init() {
            _weapon = new Weapon();
            //_weapon.InitWeapon(null, _stats.ammoCapacity, _stats.ammoCapacity, _stats.reloadTime, _stats.attackSpeed);

            _stats.hp = 1000f;
            _stats.curHp = 1000f;
        }

        public void Damage(float damage) {
            _stats.curHp -= damage;
            _updateHpAction?.Invoke(_stats.hp, _stats.curHp);
        }

        public Vector3 HitPosition() {
            if (_hitIndex >= _hitAreas.Length) _hitIndex = Random.Range(0, _hitAreas.Length);
            return _hitAreas[_hitIndex].transform.position;
        }


        public void AddUpdateHpHandler(Action<float, float> action) {
            _updateHpAction += action;
        }
    }
}
