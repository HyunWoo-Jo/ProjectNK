using UnityEngine;
using N.DesignPattern;
using Firebase.Database;
using Cysharp.Threading.Tasks;
using System;
namespace N.Network
{
    public class FirebaseManager : Singleton<FirebaseManager>
    {
        private DatabaseReference _databaseReference;


        protected override void Awake() {
            base.Awake();
            _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public async void ReadSkillData(Action<string> callback) {
            Debug.Log("hellol");
            DataSnapshot snapshot = await _databaseReference.Child("CharacterData").GetValueAsync().AsUniTask();

            if (snapshot.Exists) {
                Debug.Log("전체데이터" + snapshot.GetRawJsonValue());
                callback(snapshot.GetRawJsonValue());
            }
        }
    }
}
