using System;
using UnityEngine;

namespace N.Data
{
    public enum EquipmentType {
        Head,
        Armor,
        Glove,
        Shoes,
    }
    [Serializable]
    public class Equipment {
        public int firebaseIndex; // 인덱스
        public string name; // 이름
        public EquipmentType type; // 종류
        public float point; // 방어 수치
    }
}
