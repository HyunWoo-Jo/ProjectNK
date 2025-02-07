using N.DesignPattern;
using UnityEngine;
namespace N.Game
{
    public enum Owner {
        Player,
        Enemy,
    }

    public class Bullet : MonoBehaviour
    {
        private Owner _owner;
        private ObjectPoolItem _item;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private Vector3 _targetPos;
        private float _damage;

        private void Start() {
            _item = GetComponent<ObjectPoolItem>();
        }
        internal void SetOwner(Owner owner) {
            this._owner = owner;
       
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

        private void OnTriggerEnter(Collider other) {
            if (_owner.Equals(Owner.Player)) {
                if (other.CompareTag("Enemy")) {
                    other.GetComponent<Enemy>().Damage(_damage);
                    _item.Repay();
                    this.gameObject.SetActive(false);
                }
            } else if (_owner.Equals(Owner.Enemy)) {
                

            }
        }
    }
}
