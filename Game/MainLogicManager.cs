using UnityEngine;
using N.DesignPattern;
using System;
using System.Collections.Generic;
using N.Data;
namespace N.Game
{
    /// <summary>
    /// ���� ���� ������ �Ѱ��ִ� ������ ����
    /// </summary>
    public class MainLogicManager : Singleton<MainLogicManager> {
        public CameraLogicClassName _cameraLogicClassName = CameraLogicClassName.StandardGameCameraLogic;
        public CombatLogicClassName _combatLogicClassName = CombatLogicClassName.StandardCombatLogic;
        public List<InputLogicClassName> _inputLogicClassName_list = new List<InputLogicClassName> { InputLogicClassName.InputCombatAimLogic };
        public List<string> characterName_list = new List<string> { "Lux", "Nami", "Nunu", "Ryze" };
        public PlayState playState;
        public PlayMainLogic curPlayMainLogic;
        protected override void Awake() {
            base.Awake();
            Application.targetFrameRate = Settings.targetFrame;
        }
        /// <summary>
        /// �Լ���,Ŭ���������� �Լ� ȣ��
        /// </summary>
        /// <param name="mainLogic"></param>
        /// <param name="methodName"></param>
        /// <param name="className"></param>
        private void InvokeGenericMethod(PlayMainLogic mainLogic, string methodName, string className) {
            Type type = Type.GetType($"N.Game.{className}");
            var setCameraMethod = mainLogic.GetType().GetMethod(methodName).MakeGenericMethod(type);
            setCameraMethod.Invoke(mainLogic, null);
        }
        /// <summary>
        /// playlogic �ʱ�ȭ �Լ� play������ PlayMainLogic.cs �����ɋ� ȣ��
        /// </summary>
        /// <param name="mainLogic"></param>
        internal void SendModules(PlayMainLogic mainLogic) {
            InvokeGenericMethod(mainLogic, "SetCamera", _cameraLogicClassName.ToString());

            foreach(var inputClassName in _inputLogicClassName_list) {
                InvokeGenericMethod(mainLogic, "SetInput", inputClassName.ToString());
            }

            InvokeGenericMethod(mainLogic, "SetCombat", _combatLogicClassName.ToString());
        }


       
    }
}
