using UnityEngine;
using Firebase.Extensions;
using Firebase.Database;

namespace N.Network
{
#if UNITY_EDITOR
    public class FirebaseDataAdder : MonoBehaviour
    {
        public struct Chracter {
            public string characterName;
            public float attack;
            public float armor;
            public float hp;
            public float curHp;
            public float skilDmg;
            public float attackSpeed;
            public float ammoCapacity;
            public float reloadTime;

            public string modelName;
            public string potraitName;
            public string bulletName;

        }

        DatabaseReference reference;

        private void Start() {
            reference = FirebaseDatabase.DefaultInstance.RootReference;
            Chracter user = new ();
            string json = JsonUtility.ToJson(user);
            reference.Child("CharacterData").Child("Luxxd").SetRawJsonValueAsync(json);
        }
    }

#endif
}
