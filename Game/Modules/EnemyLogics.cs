using N.Data;
using System.Collections.Generic;
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
        private List<EnemySpawnData> _spawnData_list = new();
        public override void Instance() {
            _spawnData_list.Add(new EnemySpawnData { enemyName = EnemyName.Dragoon, hp = 100, maxHp = 100, spawnPosition = Vector3.zero, spawnTime = 2f});
        }

        public override void Work() {
            // Spawn »ý¼º
            _timer += Time.deltaTime;
            if(_spawnData_list.Count > _spawnIndex && _timer >= _spawnData_list[_spawnIndex].spawnTime) {
                EnemySpawnData data = _spawnData_list[_spawnIndex];
                string adressKey = data.enemyName.ToString() + ".prefab";
                GameObject enemyPrfab = DataManager.Instance.LoadAssetSync<GameObject>(adressKey);
                GameObject enemyObj = GameObject.Instantiate(enemyPrfab);
                enemyObj.transform.position = data.spawnPosition;
                enemyObj.transform.localScale = Vector3.one;
                _playMainLogic._fieldEnemy_list.Add(enemyObj.GetComponent<Enemy>());

                ++_spawnIndex;
            }
        }
    }
}
