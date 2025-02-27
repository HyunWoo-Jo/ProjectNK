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
using Unity.Android.Gradle.Manifest;
namespace N.Game
{

    /// <summary>
    /// �ΰ��� ���� ���� (���� ����)
    /// </summary>
    public class PlayMainLogic : MonoBehaviour
    {
        // MainLogic Generate   
        private CameraLogic _cameraLogic;
        private List<InputLogic> _inputLogic_list = new();
        private CombatLogic _combatLogic;
        private EnemyLogic _enemyLogic;

        //Data
        private InGameData _gameData;
        private List<string> _characterName_list;

        internal List<Character> _fieldCharacter_list = new();
        internal List<Enemy> _fieldEnemy_list = new();

        // Event
        private Action<Character> _changeSlotAction;
       

        //�ʱ�ȭ
        void Awake() {

            _gameData = GetComponent<InGameData>();
            MainLogicManager.Instance.curPlayMainLogic = this;
            MainLogicManager.Instance.SendModules(this);
           
            // ����� �����͸� �о�� ĳ���� ������Ʈ�� ���� �� �ʱ�ȭ
            int index = 0;
            Vector3 offset = new Vector3(0, 0, -0.04f);
         
            foreach (var characterName in _characterName_list) {
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
            _combatLogic?.Instance();
            _enemyLogic?.Instance();

            // UI ����
            foreach (var inputLogic in _inputLogic_list) {
                inputLogic.Instance();
            }
            // ���� 0����
            ChangeSlot(0);
            _fieldCharacter_list[0].ChangeState(CharacterState.Sitting);
        }

        private void OnDisable() {
            MainLogicManager.Instance.curPlayMainLogic = null;
            DataManager.Instance.ReleaseAssetAll();
        }
        void Update() {
            // Input Modules ����
            foreach (var inputLogic in _inputLogic_list) {
                inputLogic.Work();
            }
            
            _combatLogic?.Work();
            _enemyLogic?.Work();
        }

        private void LateUpdate() {
            // Camera Module ����
            _cameraLogic?.Work();
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
            inputLogic.Init(this, _gameData, UI_Controller.Instance);
            _inputLogic_list.Add(inputLogic);     
        }

        public void SetCombat<T>() where T : CombatLogic {
            _combatLogic = this.gameObject.AddComponent<T>();
            _combatLogic.Init(_gameData);
        }
        public void SetEnemy<T>() where T : EnemyLogic {
            _enemyLogic = this.gameObject.AddComponent<T>();
            _enemyLogic.Init(this, _gameData);
        }
        public void SetCharacter(List<string> characterName_list) {
            _characterName_list = characterName_list;
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
