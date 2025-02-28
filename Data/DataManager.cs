using UnityEngine;
using N.DesignPattern;
using N.Network;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;
namespace N.Data
{
    public class DataManager : Singleton<DataManager> 
    {
        private CharacterData _characterData = new();
        private Dictionary<string, object> _data_dic = new();
        private Dictionary<string, int> _dataCount_dic = new();
        private Dictionary<string, AsyncOperationHandle> _handle_dic = new();
        private Dictionary<string, EquipmentData> _equipData_dic = new();
        private bool _isAble = false;
        public bool IsAble { get { return _isAble; } }

        private void Start() {
            LoadCharacterData();
            LoadEquipmentData();
        }


        private void LoadCharacterData() {
            // Firebase Character ������ Dictionary�� ����
            FirebaseManager.Instance.ReadCharacterData(jsonData => {
                _characterData.characterData_dic = JsonConvert.DeserializeObject<Dictionary<string, CharacterStats>>(jsonData);
                _isAble = true;
            });
        }
        /// <summary>
        /// ��� ������ load
        /// </summary>
        private void LoadEquipmentData() {
            FirebaseManager.Instance.ReadUserEquipmentData(jsonData => {
                var temp_dic = JsonConvert.DeserializeObject<Dictionary<string, Equipment>>(jsonData);
                _equipData_dic = temp_dic.ToDictionary(
                    key => key.Key,
                    value => new EquipmentData {
                        key = value.Key,
                        name = value.Value.name,
                        type = value.Value.type,
                        point = value.Value.point
                    }
                 );
            });
        }
        // equipment ����
        public void RemoveEquipment(string key) {
            _equipData_dic.Remove(key);
            FirebaseManager.Instance.RemoveEquipment(key);
        }

        // equipment �߰�
        public void AddEquipment(Equipment equipment) {
            // Firebase�� �߰�
            FirebaseManager.Instance.WriteEquipment(JsonConvert.SerializeObject(equipment), (key) => {
                var eqd = equipment as EquipmentData;
                eqd.key = key;
                _equipData_dic.Add(key, eqd);
            });
        }

        /// <summary>
        /// user Character ������ ������ �߰��ؼ� ��������� �ƴϸ� CharacterData�� �������
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
