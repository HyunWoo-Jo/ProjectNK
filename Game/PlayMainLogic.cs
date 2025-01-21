using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Playables;
using N.DesignPattern;
using N.Data;
using System;
using System.Runtime.CompilerServices;
using N.UI;
namespace N.Game
{

    /// <summary>
    /// 인게임 관리 제어 (메인 로직)
    /// </summary>
    public class PlayMainLogic : MonoBehaviour
    {
        private InGameData _gameData;
        private CameraLogic _cameraLogic;
        private List<InputLogic> _inputLogic_list = new List<InputLogic>();
        private CombatLogic _combatLogic;

        private List<Character> _fieldCharacter_list = new List<Character>();
        //초기화
        void Awake() {
            _gameData = GetComponent<InGameData>();
            MainLogicManager.Instance.curPlayMainLogic = this;
            MainLogicManager.Instance.SendModules(this);
            _gameData.playState = MainLogicManager.Instance.playState;
            
            List<string> characterName_list = MainLogicManager.Instance.characterName_list;
            ChangeSlot(1);

            // 저장된 데이터를 읽어와 캐릭터 오브젝트를 생성 및 초기화
            int index = 0;
            Vector3 offset = new Vector3(0, 0, -0.04f);
            foreach (var characterName in characterName_list) {
                CharacterStats characterStats = DataManager.Instance.GetCharacterStats(characterName);
                GameObject prefab = DataManager.Instance.LoadAssetSync<GameObject>(characterStats.modelName);
                GameObject obj = Instantiate(prefab);
                obj.transform.position = _gameData.wall_list[index++].position + offset;
                _gameData.characterObj_list.Add(obj);

                Character character = obj.GetComponent<Character>();
                character.Init(_gameData, obj, characterStats, CharacterWeaponCreateSetting.Null);
                _fieldCharacter_list.Add(character);
            }

            // 전투 초기화
            _combatLogic?.InitData();

            // UI 생성
            foreach (var inputLogic in _inputLogic_list) {
                inputLogic.Instance_UI();
            }

            // reloading ui 생성 및 초기화
            GameObject reloaduiPrefab = DataManager.Instance.LoadAssetSync<GameObject>("Reload_UI.prefab");
            _gameData.reloadingUI = GameObject.Instantiate(reloaduiPrefab).GetComponent<Reloading_UI>();
            _gameData.reloadingUI.transform.SetParent(_gameData.mainCanvas.transform);
            _gameData.reloadingUI.UpdateFill(0);
            _gameData.reloadingUI.gameObject.SetActive(false);
        }

        private void OnDestroy() {
            MainLogicManager.Instance.curPlayMainLogic = null;
            DataManager.Instance.ReleaseAssetAll();
        }
        void Update() {
            // Input Modules 제어
            foreach (var inputLogic in _inputLogic_list) {
                inputLogic.WorkInput();
            }
            _combatLogic?.WorkCombat();
        }

        private void LateUpdate() {
            // Camera Module 제어
            _cameraLogic?.WorkCamera();
            //
        }

        #region public 
        /// <summary>
        /// Slot이 변경될때 호출 하는 함수
        /// </summary>
        /// <param name="index"></param>
        public void ChangeSlot(int index) {
            _cameraLogic.ChangeSlot(index);
        }
        public void Hide() {

        }

        #endregion

        #region Set modules 
        // MainLogicManager.cs 에서 invoke를 통해 컨트롤 
        public void SetCamera<T>() where T : CameraLogic {
            _cameraLogic = this.gameObject.AddComponent<T>();
            _cameraLogic.Init(Camera.main, _gameData);
        }

        public void SetInput<T>() where T : InputLogic {
            T inputLogic = InstanceComponentObject<T>();
            inputLogic.Init(_gameData);
            _inputLogic_list.Add(inputLogic);
        }

        public void SetCombat<T>() where T : CombatLogic {
            _combatLogic = this.gameObject.AddComponent<T>();
            _combatLogic.Init(_gameData);
        }
        ////////////////////////

        private T InstanceComponentObject<T>() where T : MonoBehaviour {
            GameObject obj = new() {
                isStatic = true,
                name = typeof(T).Name
            };
            return obj.AddComponent<T>();
        }
        #endregion



        
    }
}
