using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Playables;
using N.DesignPattern;
using N.Data;
using System;
using System.Runtime.CompilerServices;
using N.UI;
using NUnit.Framework;
namespace N.Game
{

    /// <summary>
    /// �ΰ��� ���� ���� (���� ����)
    /// </summary>
    public class PlayMainLogic : MonoBehaviour
    {
        // MainLogic Generate   
        private CameraLogic _cameraLogic;
        private List<InputLogic> _inputLogic_list = new List<InputLogic>();
        private CombatLogic _combatLogic;
        // Game Scene
        [SerializeField] private UI_Controller _uiController;

        //Data
        private InGameData _gameData;
        internal List<Character> _fieldCharacter_list = new List<Character>();

        // Event
        private Action<Character> _changeSlotAction;
       

        //�ʱ�ȭ
        void Awake() {
#if UNITY_EDITOR
            Assert.IsNotNull(_uiController);
#endif

            _gameData = GetComponent<InGameData>();
            MainLogicManager.Instance.curPlayMainLogic = this;
            MainLogicManager.Instance.SendModules(this);
            _gameData.playState = MainLogicManager.Instance.playState;
            
            List<string> characterName_list = MainLogicManager.Instance.characterName_list;
           
            // ����� �����͸� �о�� ĳ���� ������Ʈ�� ���� �� �ʱ�ȭ
            int index = 0;
            Vector3 offset = new Vector3(0, 0, -0.04f);
         
            foreach (var characterName in characterName_list) {
                CharacterStats characterStats = DataManager.Instance.GetCharacterStats(characterName);
                GameObject prefab = DataManager.Instance.LoadAssetSync<GameObject>(characterStats.modelName);
                GameObject obj = Instantiate(prefab);
                obj.transform.position = _gameData.wall_list[index].position + offset;
                _gameData.characterObj_list.Add(obj);

                Character character = obj.GetComponent<Character>();
                character.Init(_gameData, obj, index, characterStats, CharacterWeaponCreateSetting.Null);
                _fieldCharacter_list.Add(character);
                ++index;
            }

            // ���� �ʱ�ȭ
            _combatLogic?.InitData();

            // UI ����
            foreach (var inputLogic in _inputLogic_list) {
                inputLogic.Instance_UI();
            }
            ChangeSlot(1);
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
            _combatLogic.ChangeSlot(index);
            _changeSlotAction?.Invoke(_fieldCharacter_list[index]);
        }
        public void Hide() {

        }

        #endregion

        #region 
        public void AddChangeSlotEventHandler(Action<Character> action) {
            _changeSlotAction += action;
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
            inputLogic.Init(_gameData, _uiController);
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
