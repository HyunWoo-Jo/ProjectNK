using N.DesignPattern;
using UnityEngine;

namespace N.Game
{
    public class AutoRepay : MonoBehaviour
    {
       
        [SerializeField] private float _targetTime;
        private float _curTime;
        private ObjectPoolItem _poolItem;
        private void Update() {
            if(_poolItem == null) {
                _poolItem = GetComponent<ObjectPoolItem>();
            }
            if (_poolItem != null) {
                _curTime += Time.deltaTime;
                if (_curTime > _targetTime) {
                    _poolItem.Repay();
                    _curTime = 0;
                }
            }
        }

    }
}
