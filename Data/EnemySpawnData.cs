using UnityEngine;
using System;
using System.Collections.Generic;
namespace N.Data
{
    public enum EnemyName {
        Probe,
        Zealot,
        Dragoon,
        Arbiter
    }
    [Serializable]
    public struct EnemySpawnData {
        public float spawnTime;
        public EnemyName enemyName;
        public Vector3 spawnPosition;
        public float hp;
        public float maxHp;
        public float damage;
    }
    public class EnemySpawnDataList : ScriptableObject {
        public List<EnemySpawnData> spawnData_list;
    }
}
