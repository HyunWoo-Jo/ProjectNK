
using UnityEngine;
using N.Data;
using NUnit.Framework;
using UnityEngine.UI;
namespace N.Game
{
    [RequireComponent(typeof(Rigidbody))]
    
    public class Enemy : MonoBehaviour
    {
        private CharacterStats _stats;
        private Weapon _weapon;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _hpFill_image;
        private void Awake() {
#if UNITY_EDITOR
            Assert.IsNotNull(_canvas, "Enemy");
            Assert.IsNotNull(_hpFill_image, "Enemy");
#endif
        }
        public void Init() {
            _weapon = new Weapon();
            _weapon.InitWeapon(null, _stats.ammoCapacity, _stats.ammoCapacity, _stats.reloadTime, _stats.attackSpeed);
        }

        public void Damage(float damage) {
            _stats.curHp -= damage;
            _hpFill_image.fillAmount = _stats.curHp / _stats.hp;
        }
        
    }
}
