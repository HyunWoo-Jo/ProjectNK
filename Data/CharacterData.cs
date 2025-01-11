using UnityEngine;
using System.Collections.Generic;
namespace N.Data
{

    [System.Serializable]
    public class CharacterStats {
        public string characterName;
        public float attack;
        public float armor;
        public float hp;
        public float curHp;
        public float skilDmg;
        public float attackSpeed;
        public float ammoCapacity;

        public string modelName;
        public string potraitName;
        public string weaponType;
  
    }
    [System.Serializable]
    public class CharacterData {
        public Dictionary<string, CharacterStats> characterData_dic;

        public CharacterData() { characterData_dic = new(); }
    }
}
