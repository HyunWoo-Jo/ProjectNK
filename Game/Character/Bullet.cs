using UnityEngine;

namespace N.Game
{
    public class Bullet : MonoBehaviour
    {
        private float _moveSpeed;
        private Vector3 _targetPos;
       
        internal void SetTarget(Vector3 targetPos, float speed) {
            _moveSpeed = speed;
            _targetPos = targetPos;
            this.transform.LookAt(targetPos);
        }

        void Update()
        {
            //Move
            transform.position += transform.forward * _moveSpeed * Time.deltaTime;
        }
    }
}
