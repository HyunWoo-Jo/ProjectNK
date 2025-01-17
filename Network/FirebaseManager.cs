using UnityEngine;
using N.DesignPattern;
using Firebase.Database;
using Cysharp.Threading.Tasks;
using System;
using Firebase.Auth;
using UnityEditor;
namespace N.Network
{
    public enum FirebasePath {
        CharacterData,

    }
    [DefaultExecutionOrder(-1000)]
    public class FirebaseManager : Singleton<FirebaseManager>
    {
        private DatabaseReference _databaseReference;
        private FirebaseCAuth _firebaseAuth = new ();
        [SerializeField] private FirebaseAuthData _firebaseAuthData;

        protected override void Awake() {
            base.Awake();
            _firebaseAuth.Init(); 

            _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            GuestLogin();
        }

        // Guest �α���
        private void GuestLogin() {
            _firebaseAuthData.LoadAuthData();
            if (_firebaseAuthData.Uid.Length == 0) {
                string guestUid = SystemInfo.deviceUniqueIdentifier;
                _firebaseAuthData.Uid = SystemInfo.deviceUniqueIdentifier;
                _firebaseAuthData.Email = _firebaseAuthData.Uid + "@guest.com";
                _firebaseAuthData.Password = _firebaseAuthData.Uid;
                _firebaseAuthData.SaveAuthData();
            }
            string email = _firebaseAuthData.Email;
            string password = _firebaseAuthData.Password;

            // �α��� �õ�
            _firebaseAuth.LoginUser(email, password, () => {
                // �α��� ���н�
                _firebaseAuth.RegisterUser(email, password, null, null);
            }, null);

        }
        /// <summary>
        /// ĳ���� �����͸� �о��
        /// </summary>
        /// <param name="susCallback"></param>
        public async void ReadData(FirebasePath path, Action<string> susCallback) {
            DataSnapshot snapshot = await _databaseReference.Child(path.ToString()).GetValueAsync().AsUniTask();
            if (snapshot.Exists) {
                Debug.Log("��ü������" + snapshot.GetRawJsonValue());
                susCallback(snapshot.GetRawJsonValue());
            }
        }
    }
}
