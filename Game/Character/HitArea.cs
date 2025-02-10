using NUnit.Framework;
using UnityEngine;

namespace N.Game
{
    [RequireComponent(typeof(Rigidbody))]
    public class HitArea : MonoBehaviour
    {
        private IHitAble _hitAble;

        private void Awake() {
            _hitAble = GetComponentInParent<IHitAble>();
            if(_hitAble == null) _hitAble = GetComponent<IHitAble>();
#if UNITY_EDITOR
            Assert.IsNotNull(_hitAble, "HitArea" + gameObject.name);
#endif
        }

        public void Damage(float damage) {
            _hitAble.Damage(damage);
        }
    }
}
