using System;
using UnityEngine;

namespace N.Data
{
    public enum EquipmentType {
        All,
        Head,
        Armor,
        Belt,
        Shoes,
    }
    [Serializable]
    public class Equipment {
        public string name; // 이름
        public EquipmentType type; // 종류
        public float point; // 방어 수치
    }
    public class EquipmentData : Equipment{
        public string key; // 인덱스
    }
}
