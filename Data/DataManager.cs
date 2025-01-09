using UnityEngine;
using N.DesignPattern;
using N.Network;
using Newtonsoft.Json;
using System.Collections.Generic;
namespace N.Data
{
    public class DataManager : Singleton<DataManager> 
    {
        private CharacterData _characterData = new();
        
        private void Start() {
            LoadCharacterData();
        }

        private void LoadCharacterData() {
            // Firebase Character 정보를 Dictionary로 구성
            FirebaseManager.Instance.ReadData(FirebasePath.CharacterData, jsonData => {
                _characterData.characterData_dic = JsonConvert.DeserializeObject<Dictionary<string, CharacterStats>>(jsonData);

            });
        }
    }
}
