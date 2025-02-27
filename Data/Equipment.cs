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
        public int firebaseIndex; // �ε���
        public string name; // �̸�
        public EquipmentType type; // ����
        public float point; // ��� ��ġ
    }
}
