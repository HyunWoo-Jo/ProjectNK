
using UnityEngine;
using N.Data;
using System.Collections.Generic;
using UnityEngine.UI;
using NUnit.Framework;
using N.Utills;
using System;
using System.Linq;
namespace N.Game
{
    public enum EnemyAIType {
        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,
        Standing,
    }

    [Serializable]
    public class EnemyAI {
        public EnemyAIType type;
        public float time;
    }


    public class Enemy : MonoBehaviour, IHitAble {

        [SerializeField] private HitArea[] _hitAreas;
        [ReadOnly][SerializeField] private CharacterStats _stats;
        public CharacterStats Stats { get { return _stats; } }
        [SerializeField] private List<EnemyAI> _enemyAI_list;
        private int _aiIndex = 0;
        private float _aiTimer = 0;
        private float _aiSpeed = 0.5f;

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
        public void SetHp(float maxHp, float curHp) {
            _stats.hp = maxHp;
            _stats.curHp = curHp;
        }

        public float GetHP() => _stats.curHp;

        public void Damage(float damage) {
            _stats.curHp -= damage;
            _updateHpAction?.Invoke(_stats.hp, _stats.curHp);
        }

        public Vector3 HitPosition() {
            if (_hitIndex >= _hitAreas.Length) _hitIndex = UnityEngine.Random.Range(0, _hitAreas.Length);
            return _hitAreas[_hitIndex].transform.position;
        }


        public void AddUpdateHpHandler(Action<float, float> action) {
            _updateHpAction += action;
        }

        public void AIWork() {
            if (_enemyAI_list.Count > 0) {
                // 어느 ai를 진행할지 선택
                _aiTimer += Time.deltaTime;
                
                switch (_enemyAI_list[_aiIndex].type) {
                    case EnemyAIType.MoveLeft:
                    this.transform.position += Vector3.left * _aiSpeed * Time.deltaTime;
                    break;
                    case EnemyAIType.MoveRight:
                    this.transform.position += Vector3.right * _aiSpeed * Time.deltaTime;
                    break;
                    case EnemyAIType.MoveUp:
                    this.transform.position += Vector3.up * _aiSpeed * Time.deltaTime;
                    break;
                    case EnemyAIType.MoveDown:
                    this.transform.position += Vector3.down * _aiSpeed * Time.deltaTime;
                    break;
                    case EnemyAIType.Standing:

                    break;
                }



                // 다음 ai 검색
                if (_aiTimer >= _enemyAI_list[_aiIndex].time) {
                    _aiTimer = 0;
                    _aiIndex = _aiIndex + 1 >= _enemyAI_list.Count ? 0 : _aiIndex + 1; // loop 
                }
            }
        }
    }
}
