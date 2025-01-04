using UnityEngine;
using UnityEditor;
using N.Network;
using Firebase.Database;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
namespace N.Data
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class CharacterDataGernerater : Editor // FibaseData -> ScriteTableObject »ý¼º
    {
        [MenuItem("Generater/CharacterData")]
        public static async void CreateCharacterData() {

            var _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

            DataSnapshot snapshot = await _databaseReference.Child("CharacterData").GetValueAsync().AsUniTask();
            if (snapshot.Exists) {
                string str = snapshot.GetRawJsonValue();
                CharacterData data = new();
                data.characterData_dic = JsonConvert.DeserializeObject<Dictionary<string,CharacterStats> >(str);
                foreach (var item in data.characterData_dic) {
                    CharacterStatsSO characterStatsSO = ScriptableObject.CreateInstance<CharacterStatsSO>();
                    characterStatsSO.SetStat(item.Value);

                    string path = $"Assets/Resources/Data/Character/{item.Key}.asset";
                    AssetDatabase.CreateAsset(characterStatsSO, path);
                    AssetDatabase.SaveAssets();    
                }
                Debug.Log("Generate Character Data");
            }
        }


    }
}
