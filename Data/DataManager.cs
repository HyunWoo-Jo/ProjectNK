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
        private List<string> key_list = new();

#if UNITY_EDITOR
        //���� ���� ���������� mainScene���� firebase�� load�ϱ� ������ �ʿ���� Editor�� Test �ڵ�
        protected override void Awake() {
            base.Awake();
            string jsonData;
            if (PlayerPrefs.HasKey("editor_test")) {
               jsonData = PlayerPrefs.GetString("editor_test");       
            } else {
               jsonData = "{{\"Lux\":{{\"ammoCapacity\":0,\"armor\":0,\"attack\":0,\"attackSpeed\":0,\"characterName\":\"\",\"curHp\":0,\"hp\":0,\"modelName\":\"\",\"potraitName\":\"\",\"skilDmg\":0,\"weaponType\":\"\"}},\"Nami\":{{\"ammoCapacity\":0,\"armor\":0,\"attack\":0,\"attackSpeed\":0,\"characterName\":\"\",\"curHp\":0,\"hp\":0,\"modelName\":\"\",\"potraitName\":\"\",\"skilDmg\":0,\"weaponType\":\"\"}},\"Nunu\":{{\"ammoCapacity\":0,\"armor\":0,\"attack\":0,\"attackSpeed\":0,\"characterName\":\"\",\"curHp\":0,\"hp\":0,\"modelName\":\"\",\"potraitName\":\"\",\"skilDmg\":0,\"weaponType\":\"\"}},\"Ryze\":{{\"ammoCapacity\":0,\"armor\":0,\"attack\":0,\"attackSpeed\":0,\"characterName\":\"\",\"curHp\":0,\"hp\":0,\"modelName\":\"\",\"potraitName\":\"\",\"skilDmg\":0,\"weaponType\":\"\"}";
            }
            _characterData.characterData_dic = JsonConvert.DeserializeObject<Dictionary<string, CharacterStats>>(jsonData);
        }

#endif


        private void Start() {
            LoadCharacterData();
        }

        private void LoadCharacterData() {
            // Firebase Character ������ Dictionary�� ����
            FirebaseManager.Instance.ReadData(FirebasePath.CharacterData, jsonData => {
                _characterData.characterData_dic = JsonConvert.DeserializeObject<Dictionary<string, CharacterStats>>(jsonData);

#if UNITY_EDITOR 
                //���� ���� ���������� mainScene���� firebase�� load�ϱ� ������ �ʿ���� Editor�� Test �ڵ�
                PlayerPrefs.SetString("editor_test", jsonData);
#endif

            });
        }

        public CharacterStats GetCharacterStats(string name) => _characterData.characterData_dic[name];

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
            foreach (var key in key_list) {
                Addressables.Release(key);
            }
        }
        #endregion
    }
}
