using System;
using UnityEngine;

namespace N.DesignPattern
{
    public class ObjectPoolItem : MonoBehaviour
    {
        private IObjectPool _owner;
        private int _index;

        public ObjectPoolItem Init(IObjectPool owner, int index) {
            _owner = owner;
            _index = index;

            return this;
        }

        public void Repay() {
            _owner.RepayItem(this.gameObject, _index);
        }
    }
}
