using UnityEngine;
using N.DesignPattern;
using Firebase.Database;
using Cysharp.Threading.Tasks;
using System;
using Firebase.Auth;
using UnityEditor;
using UnityEngine.UIElements;
namespace N.Network
{
    [DefaultExecutionOrder(-1000)]
    public class FirebaseManager : Singleton<FirebaseManager>
    {
        private DatabaseReference _databaseReference;
        private FirebaseCAuth _firebaseAuth = new ();
        [SerializeField] private FirebaseAuthData _firebaseAuthData;
        private string UID { get { return _firebaseAuth.Uid; } }

        protected override void Awake() {
            base.Awake();
            _firebaseAuth.Init(); 

            _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            GuestLogin();   
        }
        

        // Guest 로그인
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

            // 로그인 시도
            _firebaseAuth.LoginUser(email, password, () => {
                // 로그인 실패시
                _firebaseAuth.RegisterUser(email, password, null, null);

            }, null);

        }
        /// <summary>
        /// 캐릭터 데이터를 읽어옴
        /// </summary>
        /// <param name="susCallback"></param>
        public async void ReadCharacterData(Action<string> susCallback) {
            DataSnapshot snapshot = await _databaseReference.Child("CharacterData").GetValueAsync().AsUniTask();
            if (snapshot.Exists) {
                Debug.Log("Read Character :" + snapshot.GetRawJsonValue());
                susCallback(snapshot.GetRawJsonValue());
            }
        }
        /// <summary>
        /// User Item 정보를 읽어옴
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="susCallback"></param>
        public async void ReadUserEquipmentData(Action<string> susCallback) {
            DataSnapshot snapshot = await _databaseReference.Child("UserData").Child(UID).Child("Equipment").GetValueAsync().AsUniTask();
            if (snapshot.Exists) {
                Debug.Log("Read User Item Data :" + snapshot.GetRawJsonValue());
                susCallback(snapshot.GetRawJsonValue());
            }
        }

        /// <summary>
        /// 아이템 추가
        /// </summary>
        /// <param name="firebaseIndex"></param>
        /// <param name="jsonData"></param>
        public async void WriteEquipment(int firebaseIndex, string jsonData) {
            DatabaseReference dataRef = _databaseReference.Child("UserData").Child(UID).Child("Equipment").Child(firebaseIndex.ToString()); // root 설정
            Debug.Log(jsonData);
            await dataRef.SetRawJsonValueAsync(jsonData);
        }

        /// <summary>
        /// Item 삭제
        /// </summary>
        /// <param name="firebaseIndex"></param>
        public async void RemoveEquipment(int firebaseIndex) {
            DatabaseReference dataRef = _databaseReference.Child("UserData").Child(UID).Child("Equipment").Child(firebaseIndex.ToString());
            await dataRef.RemoveValueAsync();
        }
    }
}
