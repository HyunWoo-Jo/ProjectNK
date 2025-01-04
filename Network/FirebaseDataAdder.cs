using UnityEngine;
using Firebase.Extensions;
using Firebase.Database;

namespace N.Network
{
#if UNITY_EDITOR
    public class FirebaseDataAdder : MonoBehaviour
    {
        public class Chracter {
            public float attack;
            public float armor;
            public float hp;
            public float skilDmg;
        }

        DatabaseReference reference;

        private void Start() {
            reference = FirebaseDatabase.DefaultInstance.RootReference;
            Chracter user = new Chracter();
            string json = JsonUtility.ToJson(user);
            reference.Child("CharacterData").Child("Nunu").SetRawJsonValueAsync(json);
            reference.Child("CharacterData").Child("Nami").SetRawJsonValueAsync(json);
            reference.Child("CharacterData").Child("Ryze").SetRawJsonValueAsync(json);
            reference.Child("CharacterData").Child("Lux").SetRawJsonValueAsync(json);
        }
    }

#endif
}
