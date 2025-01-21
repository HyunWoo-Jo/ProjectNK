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
    /// �ΰ��� ���� ���� (���� ����)
    /// </summary>
    public class PlayMainLogic : MonoBehaviour
    {
        private InGameData _gameData;
        private CameraLogic _cameraLogic;
        private List<InputLogic> _inputLogic_list = new List<InputLogic>();
        private CombatLogic _combatLogic;

        private List<Character> _fieldCharacter_list = new List<Character>();
        //�ʱ�ȭ
        void Awake() {
            _gameData = GetComponent<InGameData>();
            MainLogicManager.Instance.curPlayMainLogic = this;
            MainLogicManager.Instance.SendModules(this);
            _gameData.playState = MainLogicManager.Instance.playState;
            
            List<string> characterName_list = MainLogicManager.Instance.characterName_list;
            ChangeSlot(1);

            // ����� �����͸� �о�� ĳ���� ������Ʈ�� ���� �� �ʱ�ȭ
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

            // ���� �ʱ�ȭ
            _combatLogic?.InitData();

            // UI ����
            foreach (var inputLogic in _inputLogic_list) {
                inputLogic.Instance_UI();
            }

            // reloading ui ���� �� �ʱ�ȭ
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
            // Input Modules ����
            foreach (var inputLogic in _inputLogic_list) {
                inputLogic.WorkInput();
            }
            _combatLogic?.WorkCombat();
        }

        private void LateUpdate() {
            // Camera Module ����
            _cameraLogic?.WorkCamera();
            //
        }

        #region public 
        /// <summary>
        /// Slot�� ����ɶ� ȣ�� �ϴ� �Լ�
        /// </summary>
        /// <param name="index"></param>
        public void ChangeSlot(int index) {
            _cameraLogic.ChangeSlot(index);
        }
        public void Hide() {

        }

        #endregion

        #region Set modules 
        // MainLogicManager.cs ���� invoke�� ���� ��Ʈ�� 
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
