using N.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace N.Game
{
    public enum EnemyLogicClassName {
        StandardEnemyLogic,

    }
    public abstract class EnemyLogic : MonoBehaviour, ILogic {
        protected PlayMainLogic _playMainLogic;
        protected InGameData _gameData;
        public void Init(PlayMainLogic mainLogic, InGameData gameData) {
            _playMainLogic = mainLogic;
            _gameData = gameData;
        }

        public abstract void Instance();

        public abstract void Work();
    }

    public class StandardEnemyLogic : EnemyLogic {
        private float _timer = 0;
        private int _spawnIndex = 0;
        private List<EnemySpawnData> _spawnData_list;
        private List<Enemy> _enemy_list = new();
        public override void Instance() {
            _spawnData_list = MainLogicManager.Instance.spawnDataList.spawnData_list;
        }

        public override void Work() {
            // Spawn 생성
            _timer += Time.deltaTime;
            if(_spawnData_list.Count > _spawnIndex && _timer >= _spawnData_list[_spawnIndex].spawnTime) {
                EnemySpawnData data = _spawnData_list[_spawnIndex];
                string adressKey = data.enemyName.ToString() + ".prefab";
                GameObject enemyPrfab = DataManager.Instance.LoadAssetSync<GameObject>(adressKey);
                GameObject enemyObj = GameObject.Instantiate(enemyPrfab);
                enemyObj.transform.position = data.spawnPosition;
                enemyObj.transform.localScale = Vector3.one;
                // field에 추가
                _playMainLogic._fieldEnemy_list.Add(enemyObj.GetComponent<Enemy>());
                // 초기화
                var enemy = enemyObj.GetComponent<Enemy>();
                _enemy_list.Add(enemy);
                enemy.SetHp(data.maxHp, data.hp);
                ++_spawnIndex;
            }
            var lowHpEnemys = _playMainLogic._fieldEnemy_list.Where(e => e.GetHP() <= 0).ToList();

            // 제거
            foreach (var dieEnemy in lowHpEnemys) {
                _enemy_list.Remove(dieEnemy);
                _playMainLogic._fieldEnemy_list.Remove(dieEnemy);
                Destroy(dieEnemy.gameObject);
            }
            // ai 작동
            foreach (var enemy in _enemy_list) {
                enemy.AIWork();
            }

        }
    }
}
