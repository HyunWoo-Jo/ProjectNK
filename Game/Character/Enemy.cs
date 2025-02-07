
using UnityEngine;
using N.Data;
using NUnit.Framework;
using UnityEngine.UI;
using Unity.Plastic.Newtonsoft.Json.Serialization;
namespace N.Game
{
    [RequireComponent(typeof(Rigidbody))]
    
    public class Enemy : MonoBehaviour
    {
        private CharacterStats _stats;
        public CharacterStats Stats { get { return _stats; } }
        private Weapon _weapon;

        private Action<float, float> _updateHpAction;
        private void Awake() {

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


        public void AddUpdateHpHandler(Action<float, float> action) {
            _updateHpAction += action;
        }
    }
}
