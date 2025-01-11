using UnityEngine;
using N.DesignPattern;
using System;
using System.Collections.Generic;
using N.Data;
namespace N.Game
{
    public class MainLogicManager : Singleton<MainLogicManager>
    {
        public CameraLogicClassName _cameraLogicClassName = CameraLogicClassName.StandardGameCameraLogic;
        public List<InputLogicClassName> _inputLogicClassName_list = new List<InputLogicClassName> { InputLogicClassName.InputLimitAimLogic };
        public List<CharacterStats> character_list = new();


        /// <summary>
        /// 함수명,클래스명으로 함수 호출
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
        /// playlogic 초기화 함수 play씬에서 PlayMainLogic.cs 생성될떄 호출
        /// </summary>
        /// <param name="mainLogic"></param>
        internal void SendModules(PlayMainLogic mainLogic) {
            InvokeGenericMethod(mainLogic, "SetCamera", _cameraLogicClassName.ToString());

            foreach(var inputClassName in _inputLogicClassName_list) {
                InvokeGenericMethod(mainLogic, "SetInput", inputClassName.ToString());
            }
        }


       
    }
}
