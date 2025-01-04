using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Playables;
using N.DesignPattern;
using System;
namespace N.Game
{
    public class PlayMainLogic : MonoBehaviour
    {
        private InGameData _gameData;
        private CameraLogic _cameraLogic;
        private List<InputLogic> _inputlogic_list = new List<InputLogic>();
        //�ʱ�ȭ
        void Awake() {
            _gameData = GetComponent<InGameData>();

            MainLogicManager.Instance.SendModules(this);

            _cameraLogic.ChangeSlot(1);
        }

        #region Set modules 
        // MainLogicManager.cs ���� invoke�� ���� ��Ʈ��
        public void SetCamera<T>() where T : CameraLogic {
            _cameraLogic = this.gameObject.AddComponent<T>();
            _cameraLogic.Init(Camera.main, _gameData);
        }

        public void SetInput<T>() where T : InputLogic {
            GameObject obj = new();
            obj.isStatic = true;
            obj.name = typeof(T).Name;
            T inputLogic = obj.AddComponent<T>();
            inputLogic.Init(_gameData);
            _inputlogic_list.Add(inputLogic);
        }
        #endregion

        
        void Update() {
            // Input Modules ����
            for(int i =0;i< _inputlogic_list.Count; i++) {
                _inputlogic_list[i].WorkInput();
            }
        }

        private void LateUpdate() {
            // Camera Module ����
            _cameraLogic.WorkCamera();
            //
        }
    }
}
