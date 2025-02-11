using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using N.Utills;
namespace N.Game
{
    public abstract class CharacterAI
    {
        protected Character _owner;
        private PlayMainLogic _playMainLogic;
        public CharacterAI(Character owner, PlayMainLogic playLogic) {
            _owner = owner;
            _playMainLogic = playLogic;
        }
        
        protected List<Enemy> EnemyList() {
            return _playMainLogic._fieldEnemy_list;
        }

        internal abstract void Work();
    }
    public class CharacterStandardAI : CharacterAI {
        public CharacterStandardAI(Character owner, PlayMainLogic playLogic) : base(owner, playLogic) {
        }

        internal override void Work() {
            if (EnemyList().Count > 0) {
                // 가까운 적을 찾아옴
                if (_owner.GetAmmo > 0) {
                    Vector3 targetPos = EnemyList()
                        .Select(enemy => enemy.HitPosition())
                        .MinBy(pos => (_owner.transform.position - pos).sqrMagnitude);
                    _owner.targetPos = targetPos;
                    if (_owner.Shoot(targetPos)) _owner.ShootEvent();
                } else {
                    _owner.Reload();
                }
            } else {
                _owner.Reload();
            }
        }
    }
}
