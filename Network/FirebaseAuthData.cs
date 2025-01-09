using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System;
using N.Utills;
using UnityEditor;
namespace N.Network
{
    [CreateAssetMenu(fileName = "FirebaseAuthData", menuName = "Scriptable Objects/FirebaseAuthData")]
    public class FirebaseAuthData : ScriptableObject
    {
        [SerializeField] private string _uid;
        [SerializeField] private string _email;
        [SerializeField] private string _password;
        

        internal string Email { get { return _email; } set { _email = value; } }
        internal string Uid {  get { return _uid; } set { _uid = value; } }

        /// <summary>
        /// sha256 password
        /// </summary>
        internal string Password {
            get { return _password; }
            set {
                SHA256 sha256 = SHA256.Create();
                byte[] passwordByte = Encoding.UTF8.GetBytes(value);
                byte[] hash = sha256.ComputeHash(passwordByte);
                _password = BitConverter.ToString(hash).Replace("-","").ToLower();
            }
        }
        internal void SaveAuthData() {
            PlayerPrefs.SetString("uid", _uid);
            PlayerPrefs.SetString("email", _email);
            PlayerPrefs.SetString("password", _password);
        }

        internal void LoadAuthData() {
            if (PlayerPrefs.HasKey("uid")) {
                _uid = PlayerPrefs.GetString("uid");
                _email = PlayerPrefs.GetString("email");
                _password = PlayerPrefs.GetString("password");
            }
        }
    }
}
