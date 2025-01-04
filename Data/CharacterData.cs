using UnityEngine;
using System.Collections.Generic;
namespace N.Data
{
    [CreateAssetMenu(fileName = "CharacterStats", menuName = "Character/Stats")]
    [System.Serializable]
    public class CharacterStatsSO : ScriptableObject
    {
        public float attack;
        public float armor;
        public float hp;
        public float skilDmg;

        public void SetStat(CharacterStats stat) {
            this.attack = stat.attack;
            this.armor = stat.armor;
            this.hp = stat.hp;
            this.skilDmg = stat.skilDmg;
        }
    }
    [System.Serializable]
    public class CharacterStats {
        public float attack;
        public float armor;
        public float hp;
        public float skilDmg;
    }
    [System.Serializable]
    public class CharacterData {
        public Dictionary<string, CharacterStats> characterData_dic;

        public CharacterData() { characterData_dic = new(); }
    }
}
