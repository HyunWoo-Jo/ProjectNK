using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Playables;
using N.DesignPattern;
using N.Data;
using System;
using System.Runtime.CompilerServices;
namespace N.Game
{
    public class PlayMainLogic : MonoBehaviour
    {
        private InGameData _gameData;
        private CameraLogic _cameraLogic;
        private List<InputLogic> _inputlogic_list = new List<InputLogic>();
        private CombatLogic _combatLogic;
        //초기화
        void Awake() {
            _gameData = GetComponent<InGameData>();

            MainLogicManager.Instance.SendModules(this);
            _gameData.fieldCharacter_list = MainLogicManager.Instance.character_list;

            _cameraLogic.ChangeSlot(1);

#if UNITY_EDITOR
            // 실제 빌드시 mainScene에서 초기화 과정을 거치고 오기 때문에 필요없는 코드
            string[] names = { "Lux","Nami","Nunu","Ryze"};
            if(_gameData.fieldCharacter_list.Count == 0) {
                foreach (var name in names) {
                   _gameData.fieldCharacter_list.Add(DataManager.Instance.GetCharacterStats(name));
                }
            }
#endif

            InstanceCharacter();
        }

        private void OnDestroy() {
            DataManager.Instance.ReleaseAssetAll();
        }

        #region Set modules 
        // MainLogicManager.cs 에서 invoke를 통해 컨트롤 
        public void SetCamera<T>() where T : CameraLogic {
            _cameraLogic = this.gameObject.AddComponent<T>();
            _cameraLogic.Init(Camera.main, _gameData);
        }

        public void SetInput<T>() where T : InputLogic {
            T inputLogic = InstanceComponentObject<T>();
            inputLogic.Init(_gameData);
            _inputlogic_list.Add(inputLogic);
        }

        public void SetCombat<T>() where T : CombatLogic {
            T combatLogic = InstanceComponentObject<T>();
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

        #region Instance
        public void InstanceCharacter() {
            int index = 0;
            Vector3 offset = new Vector3(0, 0, -0.04f);
            foreach(var characterData in _gameData.fieldCharacter_list) {
                GameObject prefab = DataManager.Instance.LoadAssetSync<GameObject>(characterData.modelName);
                GameObject obj = Instantiate(prefab);
                obj.transform.position = _gameData.wall_list[index++].position + offset; 
            }
        }

        #endregion


        void Update() {
            // Input Modules 제어
            for(int i =0;i< _inputlogic_list.Count; i++) {
                _inputlogic_list[i].WorkInput();
            }
            _combatLogic?.WorkCombat();
        }

        private void LateUpdate() {
            // Camera Module 제어
            _cameraLogic?.WorkCamera();
            //
        }
    }
}
