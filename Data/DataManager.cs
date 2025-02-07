using UnityEngine;
using N.DesignPattern;
using N.Network;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
namespace N.Data
{
    public class DataManager : Singleton<DataManager> 
    {
        private CharacterData _characterData = new();
        private Dictionary<string, object> _data_dic = new();
        private Dictionary<string, int> _dataCount_dic = new();
        private Dictionary<string, AsyncOperationHandle> _handle_dic = new();
        private bool _isAble = false;
        public bool IsAble { get { return _isAble; } }

        private void Start() {
            LoadCharacterData();
        }

        private void LoadCharacterData() {
            // Firebase Character 정보를 Dictionary로 구성
            FirebaseManager.Instance.ReadData(FirebasePath.CharacterData, jsonData => {
                _characterData.characterData_dic = JsonConvert.DeserializeObject<Dictionary<string, CharacterStats>>(jsonData);
                _isAble = true;
            });
        }

        /// <summary>
        /// user Character 정보가 있으면 추가해서 가지고오고 아니면 CharacterData를 가지고옴
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CharacterStats GetCharacterStats(string name) {
            CharacterStats characterStats;
            if (_characterData.userCharacterAddStas_dic.TryGetValue(name, out CharacterStats userStats)) {
                characterStats = _characterData.characterData_dic[name] + userStats;
            } else {
                characterStats = _characterData.characterData_dic[name];
            }
            return characterStats;
        }

            #region Addressable
        public T LoadAssetSync<T>(string key) where T : Object{
            T result = null;
            if (!_data_dic.TryGetValue(key, out object obj)) {
                AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(key);
                handle.WaitForCompletion();
                result = handle.Result;
                _data_dic.Add(key, result);
                _dataCount_dic.Add(key, 1);
                _handle_dic.Add(key, handle);
            }
            if (result == null) {
                _dataCount_dic[key]++;
                return (T)obj;
            } else {
                return result;
            }
        }

        public void ReleaseAsset(string key) {
            if(_dataCount_dic.TryGetValue(key, out int count)) {
                _dataCount_dic[key]--;
                if(--count <= 0 && _handle_dic.TryGetValue(key, out var handle)) {
                    _data_dic.Remove(key);
                    _dataCount_dic.Remove(key);
                    _handle_dic.Remove(key);
                    Addressables.Release(handle);
                }
            }
        }

        public void ReleaseAssetAll() {
            foreach (var keyValue in _handle_dic) {
                Addressables.Release(keyValue.Value);
            }
            _data_dic.Clear();
            _dataCount_dic.Clear();
            _handle_dic.Clear();
        }
        #endregion
    }
}
