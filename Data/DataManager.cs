using UnityEngine;
using N.DesignPattern;
namespace N.Data
{
    public class DataManager : Singleton<DataManager> 
    {
        private CharacterData _characterData;
        
        private void Start() {
            LoadCharacterData();
        }

        private void LoadCharacterData() {

        }
    }
}
