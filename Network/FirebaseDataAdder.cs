using UnityEngine;
using Firebase.Extensions;
using Firebase.Database;

namespace N.Network
{
#if UNITY_EDITOR
    public class FirebaseDataAdder : MonoBehaviour
    {
        public class Chracter {
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

        DatabaseReference reference;

        private void Start() {
            reference = FirebaseDatabase.DefaultInstance.RootReference;
            Chracter user = new ();
            string json = JsonUtility.ToJson(user);
            reference.Child("CharacterData").Child("Nunu").SetRawJsonValueAsync(json);
            reference.Child("CharacterData").Child("Nami").SetRawJsonValueAsync(json);
            reference.Child("CharacterData").Child("Ryze").SetRawJsonValueAsync(json);
            reference.Child("CharacterData").Child("Lux").SetRawJsonValueAsync(json);
        }
    }

#endif
}
