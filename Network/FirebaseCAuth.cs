using UnityEngine;
using Firebase.Auth;
using Firebase;
using Cysharp.Threading.Tasks;
using N.Utills;
using System;
namespace N.Network
{
    public class FirebaseCAuth
    {
        private FirebaseAuth _auth;
        private FirebaseUser _user;
        
        internal string Uid {
            get { return _user.UserId; }
        }

        /// <summary>
        /// �ʱ�ȭ
        /// </summary>
        internal void Init() {
            _auth = FirebaseAuth.DefaultInstance;
         
        }

        /// <summary>
        /// ���
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="failCallback"></param>
        /// <param name="susCallback"></param>
        internal async void RegisterUser(string email, string password, Action failCallback, Action susCallback) {
            await _auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
                if (task.IsCanceled || task.IsFaulted) {
                    //���� ����
                    failCallback?.Invoke();
                    return;
                }
                _user = task.Result.User;
                susCallback?.Invoke();
            });
        }

        /// <summary>
        /// �α���
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="failCallback"></param>
        /// <param name="susCallback"></param>
        internal async void LoginUser(string email, string password, Action failCallback, Action susCallback) {
            await _auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
                if (task.IsCanceled || !task.IsFaulted) {
                    // �α��� ����
                    failCallback?.Invoke();
                    return;
                }
                _user = task.Result.User;
                susCallback?.Invoke();
            });
        }

       
    }
}
