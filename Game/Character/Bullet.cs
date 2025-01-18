using UnityEngine;

namespace N.Game
{
    public enum Owner {
        Player,
        Enemy,
    }

    public class Bullet : MonoBehaviour
    {
        private Owner owner;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private Vector3 _targetPos;
        private float _damage;
        internal void SetOwner(Owner owner) {
            this.owner = owner;
        }
        internal void SetTarget(Vector3 targetPos, float damage) {
            _targetPos = targetPos;
            _damage = damage;
            this.transform.LookAt(_targetPos);
        }

        void Update()
        {
            //Move
            transform.position += transform.forward * _moveSpeed * Time.deltaTime;
        }
    }
}
