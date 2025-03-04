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
        public string name; // �̸�
        public EquipmentType type; // ����
        public float point; // ��� ��ġ
    }
    public class EquipmentData : Equipment{
        public string key; // �ε���
    }
}
