using UnityEngine;

namespace N.DesignPattern
{
    public class ObjectPoolItem<T> : MonoBehaviour where T: MonoBehaviour
    {
        private ObjectPool<T> _owner;
        private T _t;

        public ObjectPoolItem<T> Init(ObjectPool<T> owner, T t) {
            _owner = owner;
            _t = t;
            return this;
        }

        public void Repay() {
            _owner.RepayItem(_t);
        }
    }
}
