using N.Data;
using System.Collections.Generic;
using UnityEngine;

namespace N.Game
{
    public class ChapterEnemy : MonoBehaviour {
        [SerializeField] private EnemySpawnDataList _spawnDataList;
        [SerializeField] private CameraLogicClassName _cameraLogicClassName = CameraLogicClassName.StandardGameCameraLogic;
        [SerializeField] private CombatLogicClassName _combatLogicClassName = CombatLogicClassName.StandardCombatLogic;
        [SerializeField] private EnemyLogicClassName _enemyLogicClassName = EnemyLogicClassName.StandardEnemyLogic;
        [SerializeField] private List<InputLogicClassName> _inputLogicClassName_list;

        public EnemySpawnDataList EnemySpawnDataList { get { return _spawnDataList; } }
        public CameraLogicClassName CameraLogicClassName { get { return _cameraLogicClassName; } }
        public CombatLogicClassName CombatLogicClassName { get { return _combatLogicClassName; } }
        public EnemyLogicClassName EnemyLogicClassName { get { return _enemyLogicClassName; } }
        public List<InputLogicClassName> InputLogicClassName {  get { return _inputLogicClassName_list; } }
        
    }
    

}
