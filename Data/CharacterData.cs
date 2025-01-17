using UnityEngine;
using System.Collections.Generic;
namespace N.Data
{

    public struct CharacterStats {
        public string characterName;
        public float attack;
        public float armor;
        public float hp;
        public float curHp;
        public float skilDmg;
        public float attackSpeed;
        public int ammoCapacity;
        public float reloadTime;

        public string modelName;
        public string potraitName;
        public string bulletName;
  
        public static CharacterStats operator +(CharacterStats baseStats, CharacterStats add) {
            CharacterStats b = new ();
            b.characterName = baseStats.characterName;
            b.attack = baseStats.attack + add.attack;
            b.armor = baseStats.armor + add.armor;  
            b.hp = baseStats.hp + add.hp;
            b.curHp = baseStats.curHp + add.curHp;
            b.skilDmg = baseStats.skilDmg + add.skilDmg;
            b.attackSpeed = baseStats.attackSpeed + add.attackSpeed;
            b.ammoCapacity = baseStats.ammoCapacity + add.ammoCapacity;
            b.reloadTime = baseStats.reloadTime + add.reloadTime;

            b.modelName = add.modelName != "" ? add.modelName : baseStats.modelName;
            b.potraitName = add.potraitName != "" ? add.potraitName : baseStats.potraitName;
            b.bulletName = add.bulletName != "" ? add.bulletName : baseStats.bulletName;
            return b;
        }
    }
    [System.Serializable]
    public class CharacterData {
        public Dictionary<string, CharacterStats> characterData_dic; // 캐릭터 정보
        public Dictionary<string, CharacterStats> userCharacterAddStas_dic; // 유저 캐릭터 추가 정보
        public CharacterData() { characterData_dic = new(); userCharacterAddStas_dic = new(); }
    }
}
