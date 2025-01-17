using UnityEngine;
using N.DesignPattern;
using N.Network;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
namespace N.Data
{
    public class DataManager : Singleton<DataManager> 
    {
        private CharacterData _characterData = new();
        private List<string> _key_list = new();
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
            public T LoadAssetSync<T>(string key) {
            var handle = Addressables.LoadAssetAsync<T>(key);
            handle.WaitForCompletion();
            return handle.Result;
        }

        public void ReleaseAsset(string key) {
            Addressables.Release(key);
        }

        public void ReleaseAssetAll() {
            foreach (var key in _key_list) {
                Addressables.Release(key);
            }
        }
        #endregion
    }
}
